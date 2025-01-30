import { createBrowserRouter } from "react-router-dom";
import App from "../App";
import LoginPage from "../Pages/LoginPage/LoginPage";
import RegisterPage from "../Pages/RegisterPage/RegisterPage";
import HomePage from "../Pages/HomePage.tsx/HomePage";
import UserProfile from "../Components/UserProfile/UserProfile";
import PresentationPage from "../Pages/PresentationPage/PresentationPage";

export const router = createBrowserRouter([
    {
        path: "/",
        element: <App />,
        children: [
            { path: "", element: <PresentationPage /> },
            { path: "login", element: <LoginPage /> },
            { path: "register", element: <RegisterPage /> },
            {
                path: "home", element: <HomePage />, children: [
                    { path: "profile", element: <UserProfile /> },

                ],
            },
        ],
    },
]);