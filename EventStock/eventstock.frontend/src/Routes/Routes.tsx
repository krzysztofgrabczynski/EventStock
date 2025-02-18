import { createBrowserRouter } from "react-router-dom";
import App from "../App";
import LoginPage from "../Pages/LoginPage/LoginPage";
import RegisterPage from "../Pages/RegisterPage/RegisterPage";
import HomePage from "../Pages/HomePage.tsx/HomePage";
import UserProfile from "../Components/UserProfile/UserProfile";
import PresentationPage from "../Pages/PresentationPage/PresentationPage";
import UserStocks from "../Components/UserStocks/UserStocks";
import ProtectedRoute from "./ProtectedRoute";
import AddStockUser from "../Components/AddStockUser/AddStockUser";

export const router = createBrowserRouter([
    {
        path: "/",
        element: <App />,
        children: [
            { path: "", element: <PresentationPage /> },
            { path: "login", element: <LoginPage /> },
            { path: "register", element: <RegisterPage /> },
            {
                path: "home",
                element: (
                    <ProtectedRoute>
                        <HomePage />
                    </ProtectedRoute>
                ),
                children: [
                    { path: "profile", element: <UserProfile /> },
                    { path: "stock", element: <UserStocks /> },
                    { path: "add-user-to-stock", element: <AddStockUser /> },
                ],
            },
        ],
    },
]);