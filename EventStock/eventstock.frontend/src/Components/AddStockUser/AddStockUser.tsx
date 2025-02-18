import * as Yup from "yup";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import { addUserToStockAPI } from "../../Api/apiStock";
import { Roles } from "../../Models/Stock/Roles";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../Context/useAuth";

interface Props { }

type AddUserToStockForm = {
    stockId: number;
    email: string;
    role: number;
}

const AddStockUser = (props: Props) => {
    const { user } = useAuth();
    const navigate = useNavigate();
    const validation = Yup.object().shape({
        stockId: Yup.number().required("Stock ID is required").min(0, "Missing stock error"),
        email: Yup.string().required("User email is required"),
        role: Yup.number().required("Role is required")
    });
    const {
        register,
        handleSubmit,
        formState: { errors },
    } = useForm<AddUserToStockForm>({ resolver: yupResolver(validation) });
    const handleAPI = async (form: AddUserToStockForm) => {
        await addUserToStockAPI(form);
        navigate("/home")
    }

    return (
        <div className="w-full max-w-xl pl-6 bg-white shadow-xl rounded-lg py-6 pr-6">
            <h2 className="text-2xl font-bold text-blue-600 mb-6">Add User to Stock</h2>

            <form onSubmit={handleSubmit(handleAPI)} className="space-y-4">
                <div>
                    <input
                        type="hidden"
                        value={user?.stockId || -1}
                        {...register("stockId")}
                    />
                    {errors.stockId && <p className="text-red-500">{errors.stockId.message}</p>}
                </div>

                <div>
                    <p className="text-gray-500 text-sm">User email</p>
                    <input
                        type="text"
                        {...register("email")}
                        className="text-lg font-semibold text-blue-600 border p-2 rounded w-full"
                    />
                    {errors.email && <p className="text-red-500">{errors.email.message}</p>}
                </div>

                <div>
                    <p className="text-gray-500 text-sm">Role</p>
                    <select
                        {...register("role")}
                        className="text-lg font-semibold text-blue-600 border p-2 rounded w-full"
                    >
                        {Object.keys(Roles)
                            .filter((key) => !isNaN(Number(key)))
                            .map((key) => (
                                <option key={key} value={key}>
                                    {Roles[Number(key)]}
                                </option>
                            ))}
                    </select>
                    {errors.role && <p className="text-red-500">{errors.role.message}</p>}
                </div>

                <div className="flex justify-end space-x-4 mt-6">
                    <button
                        type="submit"
                        className="px-6 py-2 bg-blue-600 text-white rounded"
                    >
                        Add User
                    </button>
                </div>
            </form>
        </div>
    );
};

export default AddStockUser;