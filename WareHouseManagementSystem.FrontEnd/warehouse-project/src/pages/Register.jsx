import { useState } from "react";
import api from "../utils/api";
import { useNavigate, Link } from "react-router-dom";

const Register = () => {
  const navigate = useNavigate();

  const [form, setForm] = useState({
    firstName: "",
    lastName: "",
    phoneNumber: "",
    address: "",
    email: "",
    password: "",
    confirmPassword: ""
  });

  const [loading, setLoading] = useState(false);

  const register = async () => {
    if (form.password !== form.confirmPassword) {
      alert("Passwords do not match");
      return;
    }

    try {
      setLoading(true);

      await api.post("/api/auth/register", form);

      navigate("/");
    } catch (err) {
      console.log(err.response?.data.errors || err.message);
      alert("Register failed");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="d-flex align-items-center justify-content-center vh-100 bg-light">

      <div className="card shadow-lg p-4" style={{ width: "400px" }}>

        <h2 className="text-center mb-4"> Create Account</h2>

        <input
          className="form-control mb-3"
          placeholder="First Name"
          onChange={(e) => setForm({ ...form, firstName: e.target.value })}
        />

         <input
          className="form-control mb-3"
          placeholder="Last Name"
          onChange={(e) => setForm({ ...form, lastName: e.target.value })}
        />
          <input
          className="form-control mb-3"
          placeholder="Phone Number"
          onChange={(e) => setForm({ ...form, phoneNumber: e.target.value })}
        />
        
          <input
          className="form-control mb-3"
          placeholder="Address"
          onChange={(e) => setForm({ ...form, address: e.target.value })}
        />

        <input
          className="form-control mb-3"
          type="email"
          placeholder="Email"
          onChange={(e) => setForm({ ...form, email: e.target.value })}
        />

        <input
          className="form-control mb-3"
          type="password"
          placeholder="Password"
          onChange={(e) => setForm({ ...form, password: e.target.value })}
        />

        <input
          className="form-control mb-3"
          type="password"
          placeholder="Confirm Password"
          onChange={(e) =>
            setForm({ ...form, confirmPassword: e.target.value })
          }
        />

        <button
          className="btn btn-success w-100"
          onClick={register}
          disabled={loading}
        >
          {loading ? "Creating..." : "Register"}
        </button>

        <div className="text-center mt-3">
          <span className="text-muted">Already have an account? </span>
          <Link to="/" className="fw-bold text-primary">
            Login
          </Link>
        </div>

      </div>
    </div>
  );
};

export default Register;