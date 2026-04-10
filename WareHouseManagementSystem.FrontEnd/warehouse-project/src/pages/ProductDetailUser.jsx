import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import api from "../utils/api";

const ProductDetailUser = () => {
  const { id } = useParams();

  const [product, setProduct] = useState(null);
  const [movements, setMovements] = useState([]);
  const [showSale, setShowSale] = useState(false);
  const [quantity, setQuantity] = useState(1);
  const [customer, setCustomer] = useState("");

  useEffect(() => {
    loadProduct();
    loadMovements();
  }, [id]);
  
  const loadProduct = async () => {
    const res = await api.get(`/api/product/${id}`);
    setProduct(res.data.data || res.data);
  };

  const loadMovements = async () => {
    try {
      const res = await api.get(`/api/StockMovement/${id}`);
      setMovements(res.data.data || res.data);
    } catch {}
  };

  const formatMovement = (m) => {
    const type = m.type?.toLowerCase();


    if (type === "transfer") {
      return `TRANSFER (${m.from} → ${m.to})`;
    }

    if (type === "processing") {
      return `PROCESSING (User → ${m.to})`;
    }

    if (type === "add") {
      return `+${m.quantity} IN (Supplier → ${m.to})`;
    }

    if (type === "completed") {
      console.log(m);
      return `${m.quantity} SALE (${m.from} → Customer: ${m.to})`;
    }

    return `${m.quantity}`;
  };

  const handleOrder = async () => {
    if (!quantity || quantity <= 0) {
      alert("Enter valid quantity");
      return;
    }

    try {
      await api.post("/api/order/create", {
        productId: Number(id),
        quantity: Number(quantity),
        customerName: customer
      });

      setShowSale(false);
      setQuantity(1);
      loadProduct();
      loadMovements();
    } catch (err) {
      alert(err.response?.data || "Error");
    }
  };

  if (!product) return <div>Loading...</div>;

  return (
    <div className="container-md mt-3">
      <div className="row g-3">
        <div className="col-md-7">
          <div className="card">
            <div className="card-body">
              <h2>{product.name}</h2>

              <div className="text-muted mb-3">
                {product.description || "No description"}
              </div>

              <span className="badge bg-blue-lt mb-3">
                {product.categoryName}
              </span>

              <div className="row text-center">
                <div className="col">
                  <div className="text-muted small">Quantity</div>
                  <div className="fw-bold">{product.quantity}</div>
                </div>

                <div className="col">
                  <div className="text-muted small">Status</div>
                  <span
                    className={`badge ${
                      product.status === "OutOfStock"
                        ? "bg-red text-white"
                        : product.status === "LowStock"
                          ? "bg-yellow text-white"
                          : "bg-green text-white"
                    }`}
                  >
                    {product.status}
                  </span>
                </div>

                <div className="col">
                  <div className="text-muted small">Location</div>
                  <span className="badge bg-purple-lt">
                    {product.locationCode}
                  </span>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div className="col-md-5">
          <div className="card">
            <div className="card-body text-center">
              <div className="text-muted">Price</div>
              <div className="display-6 fw-bold mb-2">${product.price}</div>

              <div className="text-muted mb-3">
                Total: ${quantity * product.price}
              </div>

              <button
                className="btn btn-success w-100"
                disabled={product.quantity === 0}
                onClick={() => setShowSale(true)}
              >
                Sell Product
              </button>
            </div>
          </div>
        </div>
      </div>

      <div className="card mt-3">
        <div className="card-body">
          <h4 className="mb-3">Stock History</h4>

          {movements.length === 0 && (
            <div className="text-muted">No movement yet</div>
          )}

          {movements.map((m) => (
            <div
              key={m.id}
              className="d-flex justify-content-between align-items-center py-3 border-bottom"
            >
              <div>{formatMovement(m)}</div>

              <div className="text-muted small">
                {new Date(m.date).toLocaleString()}
              </div>
            </div>
          ))}
        </div>
      </div>

      {showSale && (
        <div className="modal d-block bg-dark bg-opacity-50">
          <div className="modal-dialog modal-sm">
            <div className="modal-content">
              <div className="modal-header">
                <h5>Make Sale</h5>
                <button
                  className="btn-close"
                  onClick={() => setShowSale(false)}
                />
              </div>

              <div className="modal-body">
                <label className="form-label">Quantity</label>
                <input
                  type="number"
                  className="form-control"
                  value={quantity}
                  onChange={(e) => setQuantity(e.target.value)}
                  min="1"
                />
                <label className="form-label mt-3">Customer Name</label>
                <input
                  type="text"
                  className="form-control"
                  placeholder="Customer name"
                  value={customer}
                  onChange={(e) => setCustomer(e.target.value)}
                />

              </div>

              <div className="modal-footer">
                <button
                  className="btn btn-secondary"
                  onClick={() => setShowSale(false)}
                >
                  Cancel
                </button>

                <button className="btn btn-success" onClick={handleOrder}>
                  Confirm
                </button>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default ProductDetailUser;
