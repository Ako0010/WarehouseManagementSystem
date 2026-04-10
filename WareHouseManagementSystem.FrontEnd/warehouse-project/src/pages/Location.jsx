import { useEffect, useState } from "react";
import api from "../utils/api";

const Location = () => {
  const [locations, setLocations] = useState([]);
  const [code, setCode] = useState("");

  useEffect(() => {
    loadLocations();
  }, []);

  const loadLocations = async () => {
    try {
      const res = await api.get("/api/location");
      setLocations(res.data);
    } catch (err) {
      console.log(err);
    }
  };

  const handleCreate = async () => {
    if (!code) {
      alert("Enter location code");
      return;
    }

    try {
      await api.post("/api/location/add", { 
        Code: code
       });
      setCode("");
      loadLocations();
    } catch (err) {
      console.log(err);
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm("Delete location?")) return;

    try {
      await api.delete(`/api/location/delete/${id}`);
      loadLocations();
    } catch (err) {
      console.error(err.response.data || err.message);  
    }
  };

  return (
    <div className="container-xl mt-4">

      <div className="card mb-4">
        <div className="card-header">
          <h3>Create Location</h3>
        </div>

        <div className="card-body">
          <div className="row g-3">

            <div className="col-md-10">
              <input
                className="form-control"
                placeholder="Location Code (ex: A1-B2)"
                value={code}
                onChange={e => setCode(e.target.value)}
              />
            </div>

            <div className="col-md-2">
              <button
                className="btn btn-primary w-100"
                onClick={handleCreate}
              >
                Add
              </button>
            </div>

          </div>
        </div>
      </div>

      <div className="card">
        <div className="card-header d-flex justify-content-between">
          <h3>Locations</h3>
          <span className="badge bg-blue-lt">{locations.length}</span>
        </div>

        <div className="table-responsive">
          <table className="table table-vcenter">

            <thead>
              <tr>
                <th>Code</th>
                <th className="text-end">Action</th>
              </tr>
            </thead>

            <tbody>

              {locations.map(l => (
                <tr key={l.id}>
                  <td className="fw-bold">{l.code}</td>

                  <td className="text-end">
                    <button
                      className="btn btn-sm btn-danger"
                      onClick={() => handleDelete(l.id)}
                    >
                      Delete
                    </button>
                  </td>
                </tr>
              ))}

              {locations.length === 0 && (
                <tr>
                  <td colSpan="2" className="text-center text-muted">
                    No locations
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

export default Location;