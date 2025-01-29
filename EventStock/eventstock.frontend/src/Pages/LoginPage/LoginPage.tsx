import * as Yup from "yup";
import { yupResolver } from "@hookform/resolvers/yup";
import { useAuth } from "../../Context/useAuth";
import { LoginRequest } from "../../Models/Auth/LoginRequest";
import { useForm } from "react-hook-form";

type Props = {};

type LoginForm = {
    email: string;
    password: string;
};

const formValidation = Yup.object().shape({
    email: Yup.string().required("Username is required"),
    password: Yup.string().required("Password is required"),
});

const LoginPage = (props: Props) => {
    const { loginUser } = useAuth();

    const { register, handleSubmit, formState: { errors } } = useForm<LoginForm>({ resolver: yupResolver(formValidation) });

    const handleLogin = (form: LoginForm) => {
        const loginRequest: LoginRequest = {
            email: form.email,
            password: form.password,
        };
        loginUser(loginRequest);
    }

    return (
        <div className="flex items-center justify-center min-h-screen bg-gray-100 p-4">
            <div className="bg-white p-12 rounded-2xl shadow-2xl w-full max-w-md">
                <h2 className="text-3xl font-bold text-center mb-8">Login</h2>
                <form onSubmit={handleSubmit(handleLogin)} className="space-y-6">
                    <div className="flex flex-col gap-3">
                        <label className="text-gray-700 font-medium text-lg">Email</label>
                        <input
                            type="email"
                            {...register("email")}
                            className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 text-lg"
                        />
                        {errors.email && <p className="text-red-500 text-sm">{errors.email.message}</p>}
                    </div>
                    <div className="flex flex-col gap-3">
                        <label className="text-gray-700 font-medium text-lg">Password</label>
                        <input
                            type="password"
                            {...register("password")}
                            className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 text-lg"
                        />
                        {errors.password && <p className="text-red-500 text-sm">{errors.password.message}</p>}
                    </div>
                    <button
                        type="submit"
                        className="w-full bg-blue-500 text-white py-3 rounded-lg hover:bg-blue-600 transition font-medium text-lg"
                    >
                        Login
                    </button>
                </form>
            </div>
        </div>


    );
};

export default LoginPage;