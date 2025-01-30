import axios from "axios"
import { API_BASE_URL } from "./config"
import { LoginRequest } from "../Models/Auth/LoginRequest";
import { LoginResponse } from "../Models/Auth/LoginResponse";
import { RegisterRequest } from "../Models/Auth/RegisterRequest";
import { RegisterResponse } from "../Models/Auth/RegisterResponse";
import { UserProfile } from "../Models/User/UserProfile";

export const loginAPI = async (request: LoginRequest) => {
    try {
        const data = await axios.post<LoginResponse>(API_BASE_URL + "Authenticate/login", {
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
        const data = await axios.post<RegisterResponse>(API_BASE_URL + "Authenticate/register", {
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
        await axios.post<void>(API_BASE_URL + "Authenticate/logout", {
            refreshToken: refreshToken,
        });
    } catch (error) {
        console.log("error message: ", error);
        return error;
    }
}

export const getMyProfileAPI = async () => {
    try {
        const data = await axios.get<UserProfile>(API_BASE_URL + "User/my-profile");
        return data
    } catch (error) {
        console.log("error message: ", error);
        return error;
    }
}