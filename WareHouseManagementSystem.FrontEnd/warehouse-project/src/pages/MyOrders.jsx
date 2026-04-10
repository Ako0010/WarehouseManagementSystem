import { useEffect, useState } from "react";
import api from "../utils/api";

const MyOrders = () => {
  const [orders, setOrders] = useState([]);
  const [search, setSearch] = useState("");

  useEffect(() => {
    loadOrders();
  }, []);

   const filteredOrders = orders.filter(p =>
    p.product.toLowerCase().includes(search.toLowerCase())
  );


  const loadOrders = async () => {
    try {
      const res = await api.get("/api/order/my-orders"); 
      setOrders(res.data.data || res.data);
    } catch (err) {
      console.error(err);
    }
  };
   
  const processOrder = async (id) => {
    try {
      await api.post(`/api/order/processing/${id}`);
      loadOrders();
    } catch (err) {
      console.error(err);
    }
  };

  const finishOrder = async (id) => {
    try {
      await api.post(`/api/order/finish-processing/${id}`);
      loadOrders();
    } catch (err) {
      console.error(err);
    }
  };

  return (
    <div className="page">

      <div className="container-xl mt-4">

        <div className="d-flex justify-content-between mb-3">
          <h2>My Orders</h2>
          <span className="badge bg-blue-lt">{filteredOrders.length} items</span>
        </div>

        <div className="card-body">
          <input
            className="form-control mb-3"
            placeholder="Search product..."
            value={search}
            onChange={(e) => setSearch(e.target.value)}
          />
        </div>

        <div className="card">

          <div className="table-responsive">
            <table className="table table-vcenter table-hover align-middle">

              <thead>
                <tr>
                  <th>Product</th>
                  <th>Customer</th>
                  <th>Quantity</th>
                  <th>Status</th>
                  <th>Date</th>
                  <th>Is Ready</th>
                  <th>Actions</th>

                </tr>
              </thead>

              <tbody>

                {filteredOrders.map(o => (
                  console.log(o),
                  <tr key={o.id}>

                    <td className="fw-semibold">{o.product}</td>

                    <td>{o.customerName}</td>

                    <td>{o.quantity}</td>

                    <td>
                      <span className={`badge ${
                        o.status === "Pending"
                          ? "bg-yellow text-white"
                          : o.status === "Completed"
                          ? "bg-green text-white"
                          : "bg-red text-white"
                      }`}>
                        {o.status}
                      </span>
                    </td>

                    <td>
                      {new Date(o.createdAt).toLocaleDateString()}
                    </td>

                    <td>
                      {o.isReadyForProcessing}
                    </td>

                    <td>
                      {o.status === "Pending" && (
                        <button className="btn btn-primary" onClick={() => processOrder(o.id)}>
                          Process
                        </button>
                      )}
                      {o.status === "Processing" && o.isReadyForProcessing == "No" && (
                        <button className="btn btn-success" onClick={() => finishOrder(o.id)}>
                          Finish
                        </button>
                      )}


                    </td>

                  </tr>
                ))}

                {orders.length === 0 && (
                  <tr>
                    <td colSpan="5" className="text-center text-muted py-4">
                      No orders yet
                    </td>
                  </tr>
                )}

              </tbody>

            </table>
          </div>

        </div>

      </div>

    </div>
  );
};

export default MyOrders;