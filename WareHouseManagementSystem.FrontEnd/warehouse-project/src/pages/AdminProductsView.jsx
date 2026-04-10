import { useEffect, useState } from "react";
import api from "../utils/api";
import { useNavigate } from "react-router-dom";

const AdminProductsView = () => {
  const [products, setProducts] = useState([]);
  const [categories, setCategories] = useState([]);
  const [locations, setLocations] = useState([]);
  const [search, setSearch] = useState("");
  const [editingId] = useState(null);
  const navigate = useNavigate();

  const [form, setForm] = useState({
    name: "",
    description: "",
    quantity: 0,
    price: 0,
    stockLimit: 0,
    categoryId: 0
  });


   const filteredProducts = products.filter(p =>
    p.name.toLowerCase().includes(search.toLowerCase())
  );

  const getProducts = () => {
    api.get("/api/product/get-all")
      .then(res => setProducts(res.data.data));
  };

  const getCategories = () => {
    api.get("/api/category/get-all")
      .then(res => {
        const data = res.data.data || res.data;
        setCategories(data);
      });
    };

  const getLocations = () => {
    api.get("/api/location")
      .then(res => setLocations(res.data));
  };


  useEffect(() => {
    getProducts();
    getCategories();
    getLocations();
  }, []);

const handleSubmit = async (e) => {
  e.preventDefault();

  try {
    await api.post("/api/product/add", form);

    setForm({
      name: "",
      description: "",
      quantity: 0,
      price: 0,
      stockLimit: 0,
      categoryId: 0
    });

    getProducts();
  } catch (err) {
    console.log(err);
  }
};

  const handleDelete = async (id) => {
    await api.delete(`/api/product/delete/${id}`);
    getProducts();
  };

  return (
    <>
      <div className="page-header mb-3">
        <div className="container-xl">
          <h2 className="page-title">Products</h2>
        </div>
      </div>

      <div className="container-xl">

        <div className="card mb-4">
          <div className="card-header">
            <h3 className="card-title">
              {editingId ? "Update Product" : "Add Product"}
            </h3>
          </div>

          <div className="card-body">
            <form onSubmit={handleSubmit}>
              <div className="row g-3">

                <div className="col-md-2">
                  <h3>Name</h3>
                  <input
                    className="form-control"
                    value={form.name}
                    onChange={e => setForm({ ...form, name: e.target.value })}
                  />
                </div>

                <div className="col-md-2">
                  <h3>Description</h3>
                  <input
                    className="form-control"
                    value={form.description}
                    onChange={e => setForm({ ...form, description: e.target.value })}
                  />
                </div>

                <div className="col-md-2">
                  <h3>Quantity</h3>
                  <input
                    type="number"
                    min="0"
                    className="form-control"
                    value={form.quantity}
                    onChange={e => setForm({ ...form, quantity: Number(e.target.value) })}
                  />
                </div>

                <div className="col-md-2">
                  <h3>Price</h3>
                  <input
                    type="number"
                    min="0"
                    className="form-control"
                    value={form.price}
                    onChange={e => setForm({ ...form, price: Number(e.target.value) })}
                  />
                </div>
                
                 <div className="col-md-2">
                  <h3>Stock Limit</h3>
                  <input
                    type="number"
                    placeholder="Stock Limit"
                    className="form-control"
                    value={form.stockLimit}
                    onChange={(e) => setForm({ ...form, stockLimit: Number(e.target.value) })}
                  />
                </div>

                <div className="col-md-2">
                  <h3>Category</h3>
                  <select
                    className="form-select"
                    value={form.categoryId}
                    onChange={e => setForm({ ...form, categoryId: Number(e.target.value) })}
                  >
                    <option value="">Select</option>
                    {categories.map(c => (
                      <option key={c.id} value={c.id}>
                        {c.name}
                      </option>
                    ))}
                  </select>
                </div>
                
                <div className="col-md-2">
                  <h3>Location</h3>
                  <select
                    className="form-select"
                    value={form.locationId}
                    onChange={e => setForm({ ...form, locationId: Number(e.target.value) })}
                  >
                    <option value="">Select</option>
                    {locations.map(l => (
                      <option key={l.id} value={l.id}>
                        {l.code}
                      </option>
                    ))}
                  </select>
                </div>

                <div className="col-md-2 mx-128">
                  <button className="btn btn-primary w-100">
                    Add
                  </button>
                </div>

              </div>
            </form>
          </div>
        </div>

        <div className="card">
          <div className="card-header d-flex justify-content-between">
            <h3 className="card-title">All Products</h3>
            <span className="badge bg-blue-lt">{products.length} items</span>
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
                  <th>Description</th>
                  <th>Category</th>
                  <th>Qty</th>
                  <th>Price</th>
                  <th>Status</th>
                  <th>Location</th>
                  <th className="text-end">Actions</th>
                </tr>
              </thead>

              <tbody>
                {filteredProducts.map(p => (
                  <tr 
                  key={p.id}
                  className="cursor-pointer"
                  onClick={() => {navigate(`/admin/product/${p.id}`)} }>
                    <td className="fw-bold">{p.name}</td>
                    <td className="text-muted">{p.description || "-"}</td>

                    <td>
                      <span className="badge bg-blue-lt">
                        {p.categoryName || "No Category"}
                      </span>
                    </td>

                    <td>{p.quantity}</td>
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
                      <span className="badge bg-blue-lt">
                        {p.locationCode || "No Location"}
                      </span>
                    </td>

                    <td className="text-end">

                      <button
                        className="btn btn-sm btn-danger"
                        onClick={(e) => {
                          handleDelete(p.id);
                          e.stopPropagation();
                        }}
                      >
                        Delete
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>

            </table>
          </div>
        </div>

      </div>
    </>
  );
};

export default AdminProductsView;