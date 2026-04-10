import { useState } from "react";
import api from "../utils/api";
import { useTokens } from "../stores/tokenStore";
import { useNavigate } from "react-router-dom";
import { jwtDecode } from "jwt-decode";
import { Link } from "react-router-dom";

const Login = () => {
  const navigate = useNavigate();
  const setTokens = useTokens((s) => s.setTokens);

  const [form, setForm] = useState({
    email: "",
    password: ""
  });

  const [loading, setLoading] = useState(false);

  const login = async () => {
    try {
      setLoading(true);

      const res = await api.post("/api/auth/login", form);

      const { accessToken, refreshToken } = res.data.data;

      const decodedToken = jwtDecode(accessToken);

      const role =
        decodedToken[
          "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
        ] || "User";

      setTokens(accessToken, refreshToken, decodedToken);

      if (role === "Admin") {
        navigate("/admin/dashboard");
      } else {
        navigate("/user/dashboard");
      }
    } catch (err) {
      console.log(err.response?.data || err.message);
      alert("Login failed");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="d-flex align-items-center justify-content-center vh-100 bg-light">

      <div className="card shadow-lg p-4 w-95">

        <h2 className="text-center mb-4">Login</h2>

        <input
          className="form-control mb-3"
          placeholder="Email"
          onChange={(e) =>
            setForm({ ...form, email: e.target.value })
          }
        />

        <input
          className="form-control mb-3"
          type="password"
          placeholder="Password"
          onChange={(e) =>
            setForm({ ...form, password: e.target.value })
          }
        />

        <button
          className="btn btn-primary w-100"
          onClick={login}
          disabled={loading}
        >
          {loading ? "Loading..." : "Login"}
        </button>

        <div className="text-center mt-3">
        <span className="text-muted">Don't have an account? </span>
        <Link to="/register" className="text-primary fw-bold">
          Sign up
       </Link>
     </div>

      </div>

    </div>
  );
};

export default Login;