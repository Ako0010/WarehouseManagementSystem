import { Link, useNavigate } from "react-router-dom";
import { useTokens } from "../stores/tokenStore";
import { jwtDecode } from "jwt-decode";
import logo from "../assets/logo.svg";
import { IconLayoutDashboard, IconPackage, IconShoppingCart, IconCategory2, IconTransfer, IconLocation, IconArrowsTransferUp, IconPackages } from "@tabler/icons-react";

const Navbar = () => {
  const navigate = useNavigate();
  const { accessToken, clearTokens } = useTokens();

  let role = null;

  if (accessToken) {
    try {
      const decoded = jwtDecode(accessToken);
      role =
        decoded.role ||
        decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    } catch {
      role = null;
    }
  }

  const logout = async () => {
  clearTokens();
  navigate("/");
};

  return (
    <>
      <header className="navbar navbar-expand-md navbar-dark">
        <div className="container-xl">


          <Link to="/" className="navbar-brand text-white fw-bold">
            <img src={logo} alt="" />
          </Link>


          <div className="d-flex align-items-center ms-auto">

            {accessToken && (
              <>
                <span className="text-black me-6">
                  Hello {role}
                </span>

                <button
                  className="btn btn-light btn-sm bg-red text-white"
                  onClick={logout}
                >
                  Logout
                </button>
              </>
            )}

          </div>
        </div>
      </header>

      {accessToken && (
        <div className="navbar-expand-md border-bottom bg-white shadow-sm">
          <div className="container-xl">

            <ul className="nav nav-tabs border-0">

              {role === "Admin" && (
                <>
                  <li className="nav-item">
                    <Link className="nav-link" to="/admin/dashboard">
                      <IconLayoutDashboard className="me-2" />
                       Dashboard
                    </Link>
                  </li>

                  <li className="nav-item">
                    <Link className="nav-link" to="/admin/products">
                      <IconPackage className="me-2" />
                       Products
                    </Link>
                  </li>

                   <li className="nav-item">
                    <Link className="nav-link" to="/admin/category">
                      <IconCategory2 className="me-2" />
                       Category
                    </Link>
                  </li>

                    <li className="nav-item">
                    <Link className="nav-link" to="/admin/transfer">
                      <IconTransfer className="me-2" />
                       Transfer
                    </Link>
                  </li>

                  
                    <li className="nav-item">
                    <Link className="nav-link" to="/admin/location">
                      <IconLocation className="me-2" />
                       Location
                    </Link>
                  </li>

                   <li className="nav-item">
                    <Link className="nav-link" to="/admin/stock-movement">
                      <IconArrowsTransferUp className="me-2" />
                       Stock Movement
                    </Link>
                  </li>

                  <li className="nav-item">
                    <Link className="nav-link" to="/admin/orders">
                      <IconPackages className="me-2" />
                       Orders
                    </Link>
                  </li>
                </>
              )}

              {role === "User" && (
                <>
                  <li className="nav-item">
                    <Link className="nav-link" to="/user/dashboard">
                      <IconLayoutDashboard className="me-2" />
                       Dashboard
                    </Link>
                  </li>

                  <li className="nav-item">
                    <Link className="nav-link" to="/user/my-orders">
                      <IconShoppingCart className="me-2" />
                       My Operations
                    </Link>
                  </li>

                  <li className="nav-item">
                    <Link className="nav-link" to="/user/products">
                      <IconPackage className="me-2" />
                       Products
                    </Link>
                  </li>
                  
                </>
              )}

            </ul>
          </div>
        </div>
      )}
    </>
  );
};

export default Navbar;