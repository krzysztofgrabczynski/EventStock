import axios from "axios"
import { API_BASE_URL } from "./config"
import { AddUserToStock } from "../Models/Stock/AddUserToStock";
import { UpdateUserRole } from "../Models/Stock/UpdateUserRole";
import { UpdateStockAddress } from "../Models/Stock/UpdateStockAddress";
import { DeleteUserFromStock } from "../Models/Stock/DeleteUserFromStock";

export const addUserToStockAPI = async (request: AddUserToStock) => {
    try {
        await axios.post(API_BASE_URL + "Stock/add-user", {
            stockId: request.stockId,
            email: request.email,
            role: request.role
        });
    } catch (error) {
        console.log("error message: ", error);
        return error;
    }
}

export const listUserInStockAPI = async (stockId: number) => {
    try {
        const data = await axios.get(API_BASE_URL + "Stock/list-stock-users/" + stockId);
        return data;
    } catch (error) {
        console.log("error message: ", error);
        return error;
    }
}

export const editUsersRoleInStockAPI = async (request: UpdateUserRole) => {
    try {
        const data = await axios.put(API_BASE_URL + "Stock/update-user-role", {
            stockId: request.stockId,
            email: request.email,
            role: request.role,
        });
        return data;
    } catch (error) {
        console.log("error message: ", error);
        return error;
    }
}

export const editStockAddressAPI = async (request: UpdateStockAddress) => {
    try {
        const data = await axios.put(API_BASE_URL + "Stock/update-stock/" + request.stockId, {
            address: request.address,
        })
        return data;

    } catch (error) {
        console.log("error message: ", error);
        return error;
    }
}

export const deleteUserFromStockAPI = async (request: DeleteUserFromStock) => {
    try {
        await axios.delete(API_BASE_URL + "Stock/delete-user", {
            data: {
                stockId: request.stockId,
                userId: request.userId,
            }
        });
    } catch (error) {
        console.log("error message: ", error);
        return error;
    }
};