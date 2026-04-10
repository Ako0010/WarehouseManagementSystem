import { useEffect, useState } from "react";
import api from "../utils/api";

const StockMovement = () => {
  const [movements, setMovements] = useState([]);
  const [search, setSearch] = useState("");

  useEffect(() => {
    loadMovements();
  }, []);

  const loadMovements = async () => {
    try {
      const res = await api.get("/api/stockmovement");
      setMovements(res.data);
    } catch (err) {
      console.log(err);
    }
  };

   const filteredMovements = movements.filter(m =>
    m.product.toLowerCase().includes(search.toLowerCase())
  );


  const getTypeBadge = (type) => {
    if (type === "Add") return "bg-blue text-white";
    if (type === "Transfer") return "bg-yellow text-white";
    if (type === "Processing") return "bg-yellow text-white";
    return "bg-green text-white";
  };

  return (
    <div className="container-xl mt-4">

      <div className="card">

        <div className="card-header d-flex justify-content-between">
          <h3>Stock Movements</h3>
          <span className="badge bg-blue-lt">{movements.length}</span>
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
          <table className="table table-vcenter">

            <thead>
              <tr>
                <th>Product</th>
                <th>Type</th>
                <th>Quantity</th>
                <th>Location</th>
                <th>Date</th>
              </tr>
            </thead>

            <tbody>

              {filteredMovements.map(m => (
                console.log(m.type, m.to),
                <tr key={m.id}>

                  <td className="fw-bold">{m.product}</td>

                  <td>
                    <span className={`badge ${getTypeBadge(m.type)}`}>
                      {m.type}
                    </span>
                  </td>

                  <td>{m.quantity}</td>

                  <td>{m.from} → {m.to}</td>

                  <td>
                    {new Date(m.date).toLocaleString()}
                  </td>

                </tr>
              ))}

              {movements.length === 0 && (
                <tr>
                  <td colSpan="4" className="text-center text-muted">
                    No movements yet
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

export default StockMovement;