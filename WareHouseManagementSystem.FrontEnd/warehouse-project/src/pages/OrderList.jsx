import { useEffect, useState } from "react";
import api from "../utils/api";

const Orders = () => {
  const [orders, setOrders] = useState([]);
  const [search, setSearch] = useState("");

  const load = async () => {
    const res = await api.get("/api/order");
    setOrders(res.data);
  };

   const filteredOrders = orders.filter(o =>
    o.product.toLowerCase().includes(search.toLowerCase())
  );


  useEffect(() => {
    load();
  }, []);

  const complete = async (id) => {
    await api.post(`/api/order/complete/${id}`);
    load();
  };


 return (
  <div className="page-wrapper">
    <div className="container-xl mt-4">
      <div className="card">
        <div className="card-header">
          <h3 className="card-title">Orders</h3>
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
          <table className="table table-vcenter card-table">
            <thead>
              <tr>
                <th>Product</th>
                <th>Customer</th>
                <th>Qty</th>
                <th>Status</th>
                <th>Is Ready?</th>
                <th></th>
                <th></th>
              </tr>
            </thead>

            <tbody>
              {filteredOrders.map(o => {
                console.log(o.status, o.isReadyForProcessing);

                return (
                  <tr key={o.id}>
                    <td>{o.product}</td>
                    <td>{o.customerName}</td>
                    <td>{o.quantity}</td>
                    <td>
                      <span className="badge bg-blue-lt">
                        {o.status}
                      </span>
                    </td>
                    <td>
                      <span className={`badge ${o.isReadyForProcessing === "Yes" ? "text-white bg-green" : "text-white bg-red"}`}>
                        {o.isReadyForProcessing}
                      </span>
                    </td>

                    <td>
                      {o.status === "Processing" && o.isReadyForProcessing === "Yes" && (
                        <button
                          className="btn btn-success btn-sm"
                          onClick={() => complete(o.id)}
                        >
                          Complete
                        </button>
                      )}
                    </td>

                   
                  </tr>
                );
              })}
            </tbody>

          </table>
        </div>
      </div>
    </div>
  </div>
 );
};

export default Orders;