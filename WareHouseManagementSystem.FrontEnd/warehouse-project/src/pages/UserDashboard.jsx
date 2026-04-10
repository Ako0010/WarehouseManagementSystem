import { useEffect, useState } from "react";
import api from "../utils/api";
import Chart from "react-apexcharts";

const UserDashboard = () => {
  const [products, setProducts] = useState([]);
  const [orders, setOrders] = useState([]);


  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    const productRes = await api.get("/api/product/get-all");
    const orderRes = await api.get("/api/order/my-orders");

    setProducts(productRes.data.data || productRes.data);
    setOrders(orderRes.data.data || orderRes.data);
  };


  const days = [];
  const values = [];

  const completedOrders = orders.filter(s => s.status === "Completed");
  const totalSales = completedOrders.length;

   const totalRevenue = completedOrders.reduce((sum, o) => {
    const product = products.find(p => p.name === o.product);
    return sum + (product ? product.price * o.quantity : 0);
  }, 0);
  
  const todaySales = orders.filter(c =>
    new Date(c.createdAt).toDateString() === new Date().toDateString()
  ).length;


  const grouped = {};

  completedOrders.forEach(c => {
    const date = new Date(c.createdAt).toLocaleDateString();

    if (!grouped[date]) {
      grouped[date] = 0;
    }

   const product = products.find(p => p.name === c.product);
    grouped[date] += product ? product.price * c.quantity : 0;
  });


  for (let i = 6; i >= 0; i--) {
  const d = new Date();
  d.setDate(d.getDate() - i);

  const key = d.toLocaleDateString();

  days.push(key);
  values.push(grouped[key] || 0);
}

  const chartOptions = {
  chart: { toolbar: { show: false } },
  colors: ["#206bc4"],
  stroke: { width: 2 },
  grid: { strokeDashArray: 4 },
  xaxis: {
    categories: days
  }
};

const chartSeries = [
  {
    name: "Revenue",
    data: values
  }
];

  return (
    <>
      <div className="page-header mb-4">
        <div className="container-xl">
          <h2 className="page-title">Dashboard</h2>
        </div>
      </div>

      <div className="container-xl">


        <div className="row mb-4">

         <div className="col-sm-6 col-lg-4">
            <div className="card card-sm">
              <div className="card-body">
                <div className="subheader">Total Operations</div>
                <div className="h1">{totalSales}</div>
              </div>
            </div>
          </div>

          <div className="col-sm-6 col-lg-4">
            <div className="card card-sm">
              <div className="card-body">
                <div className="subheader">Today Operations</div>
                <div className="h1 text-green">{todaySales}</div>
              </div>
            </div>
          </div>

          <div className="col-sm-6 col-lg-4">
            <div className="card card-sm">
              <div className="card-body">
                <div className="subheader">Revenue</div>
                <div className="h1 text-yellow">${totalRevenue}</div>
              </div>
            </div>
          </div>

        </div>


        <div className="row">

          <div className="col-lg-7">
            <div className="card">
              <div className="card-header">
                <h3>Sales Overview</h3>
              </div>
              <div className="card-body">
                <Chart
                  options={chartOptions}
                  series={chartSeries}
                  type="area"
                  height={260}
                />
              </div>
            </div>
          </div>

          <div className="col-lg-5">
            <div className="card">
              <div className="card-header">
                <h3>Recent Operations</h3>
              </div>

              <div className="table-responsive">
                <table className="table table-vcenter">

                  <thead>
                    <tr>
                      <th>Product</th>
                      <th>Qty</th>
                      <th>Customer</th>
                    </tr>
                  </thead>

                  <tbody>

                    {completedOrders.slice(0, 5).map(s => (
                      <tr key={s.id}>
                        <td>{s.product}</td>
                        <td>{s.quantity}</td>
                        <td>{s.customerName}</td>
                        
                      </tr>
                    ))}

                    {completedOrders.length === 0 && (
                      <tr>
                        <td colSpan="3" className="text-center text-muted">
                          No operations yet
                        </td>
                      </tr>
                    )}

                  </tbody>

                </table>
              </div>

            </div>
          </div>

        </div>

      </div>
    </>
  );
};

export default UserDashboard;