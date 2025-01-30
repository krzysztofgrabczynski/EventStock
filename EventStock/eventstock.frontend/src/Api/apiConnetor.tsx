import axios from "axios"
import { API_BASE_URL } from "./config"
import { LoginRequest } from "../Models/Auth/LoginRequest";
import { LoginResponse } from "../Models/Auth/LoginResponse";
import { RegisterRequest } from "../Models/Auth/RegisterRequest";
import { RegisterResponse } from "../Models/Auth/RegisterResponse";

export const loginAPI = async (request: LoginRequest) => {
    try {
        const data = await axios.post<LoginResponse>(API_BASE_URL + "Authenticate/Login", {
            email: request.email,
            password: request.password
        });
        return data;
    } catch (error) {
        console.log("error message: ", error);
        return error;
    }
}

export const registerAPI = async (request: RegisterRequest) => {
    try {
        const data = await axios.post<RegisterResponse>(API_BASE_URL + "Authenticate/Register", {
            email: request.email,
            firstName: request.firstName,
            lastName: request.lastName,
            password: request.password,
            confirmPassword: request.confirmPassword,
        });
        return data;
    } catch (error) {
        console.log("error message: ", error);
        return error
    }
}

export const logoutAPI = async (refreshToken: string) => {
    try {
        await axios.post<void>(API_BASE_URL + "Authenticate/Logout", {
            refreshToken: refreshToken,
        });
    } catch (error) {
        console.log("error message: ", error);
        return error;
    }
}