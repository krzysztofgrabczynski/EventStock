import axios from "axios";
import React from "react";
import { createContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { loginAPI, logoutAPI, registerAPI } from "../Api/apiConnetor";
import { LoginRequest } from "../Models/Auth/LoginRequest";
import { RegisterRequest } from "../Models/Auth/RegisterRequest";
import { UserProfile } from "../Models/User/UserProfile";
import { getMyProfileAPI, updateMyProfileAPI } from "../Api/apiUser";

type AuthContextType = {
    token: string | null;
    refreshToken: string | null;
    user: UserProfile | null;
    updateUser: (updatedUser: UserProfile) => void;
    registerUser: (registerRequest: RegisterRequest) => void;
    loginUser: (loginRequest: LoginRequest) => void;
    logoutUser: () => void;
    isLoggedIn: () => boolean;
};

type Props = { children: React.ReactNode };

const AuthContext = createContext<AuthContextType>({} as AuthContextType);

export const AuthProvider = ({ children }: Props) => {
    const navigate = useNavigate();
    const [token, setToken] = useState<string | null>(null);
    const [refreshToken, setRefreshToken] = useState<string | null>(null);
    const [user, setUser] = useState<UserProfile | null>(null);
    const [isReady, setIsReady] = useState(false);

    useEffect(() => {
        const user = localStorage.getItem("user");
        const token = localStorage.getItem("token");
        const refreshToken = localStorage.getItem("refreshToken");
        if (user && token && refreshToken) {
            setUser(JSON.parse(user));
            setToken(token);
            setRefreshToken(refreshToken);
            axios.defaults.headers.common["Authorization"] = "Bearer " + token;
        }
        setIsReady(true);
    }, []);

    const registerUser = async (registerRequest: RegisterRequest) => {
        await registerAPI(registerRequest);
        navigate("/login");
    }

    const loginUser = async (loginRequest: LoginRequest) => {
        try {
            const loginResponse = await loginAPI(loginRequest)
            const userResponse = await getMyProfileAPI(loginResponse.data.accessToken);
            if (loginResponse && userResponse) {
                const userObj: UserProfile = {
                    email: userResponse.data.email,
                    firstName: userResponse.data.firstName,
                    lastName: userResponse.data.lastName,
                };

                localStorage.setItem("token", loginResponse.data.accessToken);
                localStorage.setItem("refreshToken", loginResponse.data.refreshToken);
                localStorage.setItem("user", JSON.stringify(userObj));

                setToken(loginResponse.data.accessToken);
                setRefreshToken(loginResponse.data.refreshToken);
                setUser(userObj);

                navigate("/home")
            };
        } catch (error) {
            console.log("error message: ", error);
            return error;
        }
    };

    const logoutUser = async () => {
        await logoutAPI(refreshToken!);
        localStorage.removeItem("token");
        localStorage.removeItem("refreshToken");
        localStorage.removeItem("user");
        setToken(null);
        setRefreshToken(null);
        setUser(null);
        navigate("/login");
    }

    const isLoggedIn = () => {
        return !!token;
    }

    const updateUser = async (updatedUser: UserProfile) => {
        await updateMyProfileAPI(updatedUser);
        localStorage.setItem("user", JSON.stringify(updatedUser));
        setUser(updatedUser);
    }

    return (
        <AuthContext.Provider value={{ token, refreshToken, registerUser, user, updateUser, loginUser, logoutUser, isLoggedIn }}>
            {isReady ? children : null}
        </AuthContext.Provider>
    );
};

export const useAuth = () => React.useContext(AuthContext);