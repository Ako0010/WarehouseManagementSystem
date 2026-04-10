import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import api from "../utils/api";

const ProductDetailAdmin = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  const [product, setProduct] = useState(null);
  const [editMode, setEditMode] = useState(false);
  const [categories, setCategories] = useState([]);
  const [locations, setLocations] = useState([]);

  const [form, setForm] = useState({
    name: "",
    description: "",
    quantity: 0,
    price: 0,
    categoryId: 0,
    locationId: 0
  });

  const getProduct = async () => {
    try {
      const res = await api.get(`/api/product/${id}`);
      const data = res.data.data || res.data;
      setProduct(data);
      setForm({
        name: data.name,
        description: data.description || "",
        quantity: data.quantity,
        price: data.price,
        categoryId: data.categoryId ?? 0,
        locationId: data.locationId ?? 0
      });
    } catch {
      alert("Product not found");
      navigate("/products");
    }
  };
  const getCategories = async () => {
    try {
      const res = await api.get("/api/category/get-all");
      setCategories(res.data.data || res.data);
    } catch (err) {
      console.error(err);
    }
  };
  const getLocations = async () => {
    try {
      const res = await api.get("/api/location");
      setLocations(res.data.data || res.data);
    } catch (err) {
      console.error(err);
    }
  };

  useEffect(() => {
    getProduct();
    getCategories();
    getLocations();
  }, []);

  const handleUpdate = async () => {
    try {
      await api.put(`/api/product/update/${id}`, form);
      setEditMode(false);
      getProduct();
    } catch {
      alert("Update failed");
    }
  };

  const handleDelete = async () => {
    const confirmDelete = window.confirm(
      "Are you sure you want to delete this product?",
    );
    if (!confirmDelete) return;

    try {
      await api.delete(`/api/product/delete/${id}`);
      navigate("/admin/products");
    } catch {
      alert("Delete failed");
    }
  };

  if (!product) return <div className="container mt-4">Loading...</div>;

  return (
    <div className="container-xl mt-4">
      <div className="card shadow-sm">
        <div className="card-header d-flex justify-content-between">
          <h3 className="card-title">Product Details</h3>

          <div>
            <button
              className="btn btn-warning me-2"
              onClick={() => setEditMode(!editMode)}
            >
              {editMode ? "Cancel" : "Edit"}
            </button>

            <button className="btn btn-danger" onClick={handleDelete}>
              Delete
            </button>
          </div>
        </div>

        <div className="card-body">
          <div className="row g-3">
            <div className="col-md-6">
              <label className="form-label">Name</label>
              <input
                className="form-control"
                value={form.name}
                disabled={!editMode}
                onChange={(e) => setForm({ ...form, name: e.target.value })}
              />
            </div>

            <div className="col-md-6">
              <label className="form-label">Price</label>
              <input
                type="number"
                className="form-control"
                value={form.price}
                disabled={!editMode}
                onChange={(e) => setForm({ ...form, price: e.target.value })}
              />
            </div>

            <div className="col-md-6">
              <label className="form-label">Quantity</label>
              <input
                type="number"
                className="form-control"
                value={form.quantity}
                disabled={!editMode}
                onChange={(e) => setForm({ ...form, quantity: e.target.value })}
              />
            </div>

            <div className="col-md-6">
              <label className="form-label">Category</label>

              {editMode && (
                <select
                  className="form-select"
                  value={form.categoryId ?? ""}
                  onChange={(e) =>
                    setForm({
                      ...form,
                      categoryId: Number(e.target.value),
                    })
                  }
                >
                  <option value="">Select category</option>

                  {categories.map((c) => (
                    <option key={c.id} value={c.id}>
                      {c.name}
                    </option>
                  ))}
                </select>
              )}

              {!editMode && (
                <div className="form-control bg-light">
                  {product?.categoryName || "No Category"}
                </div>
              )}
            </div>

            <div className="col-12">
              <label className="form-label">Description</label>
              <textarea
                className="form-control"
                rows="3"
                value={form.description}
                disabled={!editMode}
                onChange={(e) =>
                  setForm({ ...form, description: e.target.value })
                }
              />
            </div>
          </div>

          {editMode && (
            <div className="mt-4">
              <button className="btn btn-primary w-100" onClick={handleUpdate}>
                Save Changes
              </button>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default ProductDetailAdmin;
