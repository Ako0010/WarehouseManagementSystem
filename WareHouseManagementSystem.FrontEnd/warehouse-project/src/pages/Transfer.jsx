import { useEffect, useState } from "react";
import api from "../utils/api";

const Transfer = () => {
  const [products, setProducts] = useState([]);
  const [locations, setLocations] = useState([]);
  const [form, setForm] = useState({
    productId: "",
    fromId: "",
    toId: "",
    quantity: 0,
  });

  useEffect(() => {
    api.get("/api/product/get-all").then((res) => setProducts(res.data.data));
    api.get("/api/location").then((res) => setLocations(res.data));
  }, []);

  const selectedProduct = products.find((p) => p.id === form.productId);

  const handleTransfer = async () => {

    if (!form.productId || !form.fromId || !form.toId) {
      alert("Fill all fields");
      return;
    }


    if (form.fromId === form.toId) {
      alert("Locations cannot be same");
      return;
    }

    if (!form.quantity || form.quantity <= 0) {
      alert("Invalid quantity");
      return;
    }

    if (selectedProduct && form.quantity > selectedProduct.quantity) {
      alert("Not enough stock");
      return;
    }

    try {
      await api.post(`/api/transfer/add`, {
        ProductId: Number(form.productId),
        FromId: Number(form.fromId),
        ToId: Number(form.toId),
        Quantity: Number(form.quantity),
      });

      alert("Transferred!");
    } catch (error) {
      console.error(error.response || error);
    }
  };

  return (
    <div className="container-xl mt-4">
      <div className="card">
        <div className="card-header">
          <h3 className="card-title">Stock Transfer</h3>
        </div>

        <div className="card-body">
          <div className="row g-3">
            <div className="col-md-6">
              <label className="form-label">Product</label>
              <select
                className="form-select"
                value={form.productId}
                onChange={(e) =>
                  setForm({ ...form, productId: Number(e.target.value) })
                }
              >
                <option value="">Select Product</option>
                {products.map((p) => (
                  <option key={p.id} value={p.id}>
                    {p.name}
                  </option>
                ))}
              </select>
            </div>

            <div className="col-md-6">
              <label className="form-label">Quantity</label>
              <input
                type="number"
                min="0"
                className="form-control"
                placeholder="Enter quantity"
                value={form.quantity}
                onChange={(e) =>
                  setForm({ ...form, quantity: Number(e.target.value) })
                }
              />
            </div>

            {selectedProduct && (
              <div className="col-12">
                <div className="alert alert-info">
                  Available Stock: <b>{selectedProduct.quantity}</b>
                  Location: <b>{selectedProduct.locationCode}</b>
                </div>
              </div>
            )}

            <div className="col-md-6">
              <label className="form-label">From Location</label>
              <select
                className="form-select"
                value={form.fromId}
                onChange={(e) =>
                  setForm({ ...form, fromId: Number(e.target.value) })
                }
              >
                <option value="">Select Location</option>
                {locations.map((l) => (
                  <option key={l.id} value={l.id}>
                    {l.code}
                  </option>
                ))}
              </select>
            </div>

            <div className="col-md-6">
              <label className="form-label">To Location</label>
              <select
                className="form-select"
                value={form.toId}
                onChange={(e) =>
                  setForm({ ...form, toId: Number(e.target.value) })
                }
              >
                <option value="">Select Location</option>
                {locations.map((l) => (
                  <option key={l.id} value={l.id}>
                    {l.code}
                  </option>
                ))}
              </select>
            </div>
          </div>    
        </div>

        <div className="card-footer text-end">
          <button
            className="btn btn-primary"
            onClick={handleTransfer}
            disabled={
              !form.productId ||
              !form.fromId ||
              !form.toId ||
              form.quantity <= 0
            }
          >
            Transfer Stock
          </button>
        </div>
      </div>
    </div>
  );
};
export default Transfer;
