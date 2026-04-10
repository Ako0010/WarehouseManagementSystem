import { Outlet } from "react-router-dom";
import Navbar from "../components/Navbar";

const MainLayout = () => {
  return (
    <div className="page">

      <Navbar />

      <div className="page-wrapper">
        <div className="page-body">
          <div className="container-xl">

            <Outlet />

          </div>
        </div>
      </div>

      <footer className="footer footer-transparent">
        <div className="container-xl text-center">
          © 2026 WMS
        </div>
      </footer>

    </div>
  );
};

export default MainLayout;