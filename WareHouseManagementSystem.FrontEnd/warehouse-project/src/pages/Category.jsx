import { useEffect, useState } from "react";
import api from "../utils/api";

const CategoryAdd = () => {
  const [categories, setCategories] = useState([]);
  const [name, setName] = useState("");

  const getCategories = () => {
    api.get("/api/category/get-all")
      .then(res => {
        const data = res.data.data || res.data;
        setCategories(data);
      })
      .catch(err => console.log(err));
  };

  useEffect(() => {
    getCategories();
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!name.trim()) {
      alert("Category name cannot be empty");
      return;
    }

    try {
      await api.post("/api/category/add", { name });
      setName("");
      getCategories();
    } catch {
      alert("Xəta baş verdi");
    }
  };

 const handleDelete = async (id) => {
    const confirmDelete = window.confirm("Are you sure you want to delete this category?");
    if (!confirmDelete) return;

    try {
      await api.delete(`/api/category/delete/${id}`);
      getCategories();
    } catch {
      alert("Delete failed");
    }
  };

  return (
    <div className="container-xl mt-4">

      <div className="card mb-4 shadow-sm">
        <div className="card-header">
          <h3 className="card-title">Add Category</h3>
        </div>

        <div className="card-body">
          <form onSubmit={handleSubmit}>
            <div className="row g-3 align-items-center">

              <div className="col-md-9">
                <input
                  className="form-control"
                  placeholder="Category name..."
                  value={name}
                  onChange={(e) => setName(e.target.value)}
                />
              </div>

              <div className="col-md-3">
                <button className="btn btn-primary w-100">
                  Add
                </button>
              </div>

            </div>
          </form>
        </div>
      </div>

      <div className="card shadow-sm">
        <div className="card-header d-flex justify-content-between align-items-center">
          <h3 className="card-title">Categories</h3>
          <span className="badge bg-blue-lt">{categories.length} items</span>
        </div>

        <div className="table-responsive">
          <table className="table table-vcenter table-hover align-middle">
            <thead className="table-light">
              <tr>
                <th className="w-60px">#</th>
                <th>Name</th>
                <th className="text-end">Action</th>
              </tr>
            </thead>

            <tbody>
              {categories.map((c, index) => (
                <tr key={c.id}>
                  <td className="text-muted">{index + 1}</td>

                  <td className="fw-semibold">
                    {c.name}
                  </td>

                  <td className="text-end">
                    <button
                      className="btn btn-sm btn-danger"
                      onClick={() => handleDelete(c.id)}
                    >
                      Delete
                    </button>
                  </td>
                </tr>
              ))}

              {categories.length === 0 && (
                <tr>
                  <td colSpan="3" className="text-center text-muted py-4">
                    No categories found
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

export default CategoryAdd;