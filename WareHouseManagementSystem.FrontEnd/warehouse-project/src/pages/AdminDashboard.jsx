import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../utils/api";
import Chart from "react-apexcharts";

const AdminDashboard = () => {
  const [products, setProducts] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    api.get("/api/product/get-all")
      .then(res => setProducts(res.data.data))
      .catch(err => console.error(err));
  }, []);

  const total = products.length;
  const inStock = products.filter(p => p.status !== "OutOfStock").length;
  const lowStock = products.filter(p => p.status === "LowStock").length;
  const outStock = products.filter(p => p.status === "OutOfStock").length;

  const categories = {};

  products.forEach(p => {
    const cat = p.categoryName || "Other";
    categories[cat] = (categories[cat] || 0) + 1;
  });

  const categoryNames = Object.keys(categories);
  const categoryCounts = Object.values(categories);

  const stockOptions = {
    chart: {
      toolbar: { show: false },
      fontFamily: "inherit"
    },
    colors: ["#206bc4"],
    stroke: { width: 2 },
    grid: {
      strokeDashArray: 4
    },
    xaxis: {
      categories: ["In Stock", "Out Of Stock","Low Stock"]
    }
  };

  const stockSeries = [
    {
      name: "Products",
      data: [inStock, outStock,lowStock]
    }
  ];

  return (
    <>
      <div className="page-header d-print-none mb-4">
        <div className="container-xl">
          <h2 className="page-title">Dashboard</h2>
        </div>
      </div>

      <div className="container-xl">

        <div className="row row-deck row-cards mb-4">

          <div className="col-sm-6 col-lg-4">
            <div className="card card-sm">
              <div className="card-body">
                <div className="subheader">Total Products</div>
                <div className="h1">{total}</div>
              </div>
            </div>
          </div>

          <div className="col-sm-6 col-lg-4">
            <div className="card card-sm">
              <div className="card-body">
                <div className="subheader">In Stock</div>
                <div className="h1 text-green">{inStock}</div>
              </div>
            </div>
          </div>

          <div className="col-sm-6 col-lg-4">
            <div className="card card-sm">
              <div className="card-body">
                <div className="subheader">Out Of Stock</div>
                <div className="h1 text-red">{outStock}</div>
              </div>
            </div>
          </div>

           <div className="col-sm-6 col-lg-4">
            <div className="card card-sm">
              <div className="card-body">
                <div className="subheader">Low Stock</div>
                <div className="h1 text-yellow">{lowStock}</div>
              </div>
            </div>
          </div>

        </div>

        <div className="row row-cards mb-4">

          <div className="col-lg-6">
            <div className="card">
              <div className="card-header">
                <h3 className="card-title">Stock Overview</h3>
              </div>
              <div className="card-body">
                <Chart
                  options={stockOptions}
                  series={stockSeries}
                  type="line"
                  height={260}
                />
              </div>
            </div>
          </div>

          <div className="col-lg-6">
            <div className="card">
              <div className="card-header">
                <h3 className="card-title">Products by Category</h3>
              </div>
              <div className="card-body">
                <Chart
                  options={{ labels: categoryNames }}
                  series={categoryCounts}
                  type="pie"
                  height={260}
                />
              </div>
            </div>
          </div>

        </div>

        <div className="card">
          <div className="card-header">
            <h3 className="card-title">Recent Products</h3>
          </div>

          <div className="table-responsive">
            <table className="table table-vcenter table-hover">
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
                {products.map(p => (
                  <tr 
                  key={p.id}
                  className="cursor-pointer"
                  onClick={() => navigate(`/admin/product/${p.id}`)}>

                    <td>{p.name}</td>

                    <td>
                      <span className="badge bg-blue-lt">
                        {p.categoryName || "No Category"}
                      </span>
                    </td>

                    <td>{p.quantity}</td>
                    <td>{p.price}</td>

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

export default AdminDashboard;