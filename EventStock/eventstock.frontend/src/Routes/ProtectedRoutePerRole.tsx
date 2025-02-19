import React from "react";
import { Navigate, useLocation } from "react-router-dom";
import { useAuth } from "../Context/useAuth";

type Props = { children: React.ReactNode };

const ProtectedRoutePerRole = ({ children }: Props) => {
    const location = useLocation();
    const { user } = useAuth();

    return (user?.roles.includes("StockAdmin") || user?.roles.includes("StockModerator")) ? (
        <> {children} </>
    ) : (
        <Navigate to="/home" state={{ from: location }} replace />
    );
};

export default ProtectedRoutePerRole;