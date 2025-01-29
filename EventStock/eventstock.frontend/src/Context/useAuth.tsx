import axios from "axios";
import React from "react";
import { createContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { loginAPI, logoutAPI, registerAPI } from "../Api/apiConnetor";
import { LoginRequest } from "../Models/Auth/LoginRequest";
import { RegisterRequest } from "../Models/Auth/RegisterRequest";

type AuthContextType = {
    token: string | null;
    refreshToken: string | null;
    registerUser: (registerRequest: RegisterRequest) => void;
    loginUser: (loginRequest: LoginRequest) => void;
    logoutUser: (refreshToken: string) => void;
    isLoggedIn: () => boolean;
};

type Props = { children: React.ReactNode };

const AuthContext = createContext<AuthContextType>({} as AuthContextType);

export const AuthProvider = ({ children }: Props) => {
    const navigate = useNavigate();
    const [token, setToken] = useState<string | null>(null);
    const [refreshToken, setRefreshToken] = useState<string | null>(null);
    const [isReady, setIsReady] = useState(false);

    useEffect(() => {
        const token = localStorage.getItem("token");
        const refreshToken = localStorage.getItem("refreshToken");
        if (token && refreshToken) {
            setToken(token);
            setRefreshToken(refreshToken);
            axios.defaults.headers.common["Authorization"] = "Bearer " + token;
        }
        setIsReady(true);
    }, []);

    const registerUser = async (registerRequest: RegisterRequest) => {
        const response = await registerAPI(registerRequest);
        navigate("/login");
    }

    const loginUser = async (loginRequest: LoginRequest) => {
        await loginAPI(loginRequest).then((response) => {
            if (response) {
                localStorage.setItem("token", response.data.accessToken);
                localStorage.setItem("refreshToken", response.data.refreshToken);

                setToken(response.data.accessToken);
                setRefreshToken(response.data.refreshToken);
                navigate("/")
            }
        }).catch((e) => console.log("error in login useAuth: ", e));
    };

    const logoutUser = async (refreshToken: string) => {
        await logoutAPI(refreshToken);
        localStorage.removeItem("token");
        localStorage.removeItem("refreshToken");
        setToken(null);
        setRefreshToken(null);
        navigate("/login");
    }

    const isLoggedIn = () => {
        return !!token;
    }

    return (
        <AuthContext.Provider value={{ token, refreshToken, registerUser, loginUser, logoutUser, isLoggedIn }}>
            {isReady ? children : null}
        </AuthContext.Provider>
    );
};

export const useAuth = () => React.useContext(AuthContext);