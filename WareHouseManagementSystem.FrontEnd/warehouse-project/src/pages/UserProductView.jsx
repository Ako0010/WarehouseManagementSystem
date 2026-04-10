import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../utils/api";

const UserProductView = () => {
  const [products, setProducts] = useState([]);
  const [search, setSearch] = useState("");
  const navigate = useNavigate();

  const loadProducts = async () => {
    try {
      const res = await api.get("/api/product/get-all");
      setProducts(res.data.data || res.data);
    } catch (err) {
      console.log(err);
    }
  };

  useEffect(() => {
    loadProducts();
  }, []);

  const filteredProducts = products.filter(p =>
    p.name.toLowerCase().includes(search.toLowerCase())
  );

  return (
    <div className="container-xl mt-3">

      <div className="card">

        <div className="card-header d-flex justify-content-between align-items-center">
          <h3 className="card-title">Products</h3>
          <span className="badge bg-blue-lt">
            {filteredProducts.length} items
          </span>
        </div>

        <div className="card-body">
          <input
            className="form-control mb-3"
            placeholder="Search product..."
            value={search}
            onChange={(e) => setSearch(e.target.value)}
          />
        </div>

        <div className="table-responsive">
          <table className="table table-vcenter table-hover align-middle">

            <thead>
              <tr>
                <th>Name</th>
                <th>Category</th>
                <th>Quantity</th>
                <th>Price</th>
                <th>Status</th>
                <th>Location</th>
              </tr>
            </thead>

            <tbody>
              {filteredProducts.map(p => (
                <tr
                  key={p.id}
                  style={{ cursor: "pointer" }}
                  className="hover-shadow"
                  onClick={() => navigate(`/user/product/${p.id}`)}
                >
                  <td className="fw-bold">{p.name}</td>

                  <td>
                    <span className="badge bg-blue-lt">
                      {p.categoryName || "No Category"}
                    </span>
                  </td>

                  <td className={
                    p.quantity === 0
                      ? "text-red fw-bold"
                      : p.quantity <= p.stockLimit
                      ? "text-yellow fw-bold"
                      : ""
                  }>
                    {p.quantity}
                  </td>

                  <td>${p.price}</td>

                  <td>
                    <span className={`badge ${
                      p.status === "OutOfStock"
                        ? "bg-red text-white"
                        : p.status === "LowStock"
                        ? "bg-yellow text-white"
                        : "bg-green text-white"
                    }`}>
                      {p.status}
                    </span>
                  </td>

                  <td>
                    <span className="badge bg-purple-lt">
                      {p.locationCode || "No Location"}
                    </span>
                  </td>
                </tr>
              ))}

              {filteredProducts.length === 0 && (
                <tr>
                  <td colSpan="6" className="text-center text-muted py-4">
                    No products found
                  </td>
                </tr>
              )}

            </tbody>

          </table>
        </div>

      </div>

    </div>
  );
};

export default UserProductView;