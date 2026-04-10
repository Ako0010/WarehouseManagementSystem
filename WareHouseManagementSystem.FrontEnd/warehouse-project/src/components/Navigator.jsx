import { Routes, Route, Navigate } from "react-router-dom";
import { useTokens } from "../stores/tokenStore";
import { jwtDecode } from "jwt-decode";

import Login from "../pages/Login";
import Register from "../pages/Register";

import AdminDashboard from "../pages/AdminDashboard";
import AdminProductsView from "../pages/AdminProductsView";

import UserDashboard from "../pages/UserDashboard";
import MyOrders from "../pages/MyOrders";

import MainLayout from "../layout/MainLayout";
import Category from "../pages/Category";

import ProductDetailAdmin from "../pages/ProductDetailAdmin";
import UserProductView from "../pages/UserProductView";

import ProductDetailUser from "../pages/ProductDetailUser";
import Transfer from "../pages/Transfer";

import Location from "../pages/Location";
import StockMovement from "../pages/StockMovement";

import Orders from "../pages/OrderList";


const Navigator = () => {
  const { accessToken } = useTokens();

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

  return (
    <Routes>

      {!accessToken && (
        <>
          <Route path="/" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route path="*" element={<Navigate to="/" />} />
        </>
      )}


      {accessToken && role && (
         <Route element={<MainLayout />}>

          <Route path="/" element={
            role === "Admin"
              ? <Navigate to="/admin/dashboard" />
              : <Navigate to="/user/dashboard" />
          } />

          {role === "Admin" && (
            <>
              <Route path="/admin/dashboard" element={<AdminDashboard />} />
              <Route path="/admin/products" element={<AdminProductsView />} />
              <Route path="/admin/product/:id" element={<ProductDetailAdmin />} />
              <Route path="/admin/category" element={<Category />} />
              <Route path="/admin/transfer" element={<Transfer />} />
              <Route path="/admin/location" element={<Location />} />
              <Route path="/admin/stock-movement" element={<StockMovement />} />
              <Route path="/admin/orders" element={<Orders />} />
              

              
            </>
          )}

          {role === "User" && (
            <>
              <Route path="/user/dashboard" element={<UserDashboard />} />
              <Route path="/user/my-orders" element={<MyOrders />} />
              <Route path="/user/products" element={<UserProductView />} />
              <Route path="/user/product/:id" element={<ProductDetailUser />} />
            </>
          )}

          <Route path="*" element={<Navigate to="/" />} />
        </Route>
      )}

      <Route path="*" element={<div>Not Found</div>} />

    </Routes>
  );
};

export default Navigator;