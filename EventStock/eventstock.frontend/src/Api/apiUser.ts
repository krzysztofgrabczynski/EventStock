import axios from "axios"
import { API_BASE_URL } from "./config"
import { UserProfile } from "../Models/User/UserProfile";
import { UserPasswordUpdate } from "../Models/User/UserPasswordUpdate";

//export const getMyProfileAPI = async (accessToken: string | null = null) => {
//    try {
//        if (!accessToken) {
//            const data = await axios.get<UserProfile>(API_BASE_URL + "User/my-profile");
//            return data;
//        } else {
//            const data = await axios.get<UserProfile>(API_BASE_URL + "User/my-profile", {
//                headers: {
//                    Authorization: `Bearer ${accessToken}`
//                }
//            });
//            return data;
//        }
//    } catch (error) {
//        console.log("error message: ", error);
//        return error;
//    }
//}

export const updateMyProfileAPI = async (request: UserProfile) => {
    try {
        const data = await axios.put<UserProfile>(API_BASE_URL + "User/update-profile", {
            email: request.email,
            firstName: request.firstName,
            lastName: request.lastName,
        });
        return data;
    } catch (error) {
        console.log("error message: ", error);
        return error;
    }
}

export const changeMyPasswordAPI = async (request: UserPasswordUpdate) => {
    try {
        const data = await axios.post<UserProfile>(API_BASE_URL + "User/change-password", {
            oldPassword: request.oldPassword,
            password: request.newPassword,
            confirmPassword: request.confirmPassword,
        });
        return data;
    } catch (error) {
        console.log("error message: ", error);
        return error;
    }
}