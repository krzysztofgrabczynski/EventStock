import * as Yup from "yup";
import { yupResolver } from "@hookform/resolvers/yup";
import { useForm } from "react-hook-form";
import { useAuth } from "../../Context/useAuth";
import { RegisterRequest } from "../../Models/Auth/RegisterRequest";

type Props = {};

type RegisterForm = {
    email: string;
    firstName: string;
    lastName: string;
    password: string;
    confirmPassword: string;
}

const formValidation = Yup.object().shape({
    email: Yup.string().required("Email is required"),
    firstName: Yup.string().required("Email is required"),
    lastName: Yup.string().required("Email is required"),
    password: Yup.string()
        .min(8, "Password must be at least 8 characters")
        .matches(/[A-Z]/, "Password must contain at least one uppercase letter")
        .matches(/[0-9]/, "Password must contain at least one number")
        .matches(/[@$!%*?&]/, "Password must contain at least one special character")
        .required("Password is required"),
    confirmPassword: Yup.string()
        .oneOf([Yup.ref("password"), ""], "Passwords must match")
        .required("Confirm password is required"),
});

const RegisterPage = (props: Props) => {
    const { registerUser } = useAuth();

    const { register, handleSubmit, formState: { errors } } = useForm<RegisterForm>({ resolver: yupResolver(formValidation) });

    const handleRegister = (form: RegisterForm) => {
        const registerRequest: RegisterRequest = {
            email: form.email,
            firstName: form.firstName,
            lastName: form.lastName,
            password: form.password,
            confirmPassword: form.confirmPassword,
        };
        registerUser(registerRequest);
    }

    return (
        <div className="flex items-center justify-center min-h-screen bg-gray-100 p-4">
            <div className="bg-white p-12 rounded-2xl shadow-2xl w-full max-w-md">
                <h2 className="text-3xl font-bold text-center mb-8">Sign Up</h2>
                <form onSubmit={handleSubmit(handleRegister)} className="space-y-6">
                    <div className="flex flex-col gap-3">
                        <label className="text-gray-700 font-medium text-lg">First Name</label>
                        <input
                            type="text"
                            {...register("firstName")}
                            className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 text-lg"
                        />
                        {errors.firstName && <p className="text-red-500 text-sm">{errors.firstName.message}</p>}
                    </div>
                    <div className="flex flex-col gap-3">
                        <label className="text-gray-700 font-medium text-lg">Last Name</label>
                        <input
                            type="text"
                            {...register("lastName")}
                            className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 text-lg"
                        />
                        {errors.lastName && <p className="text-red-500 text-sm">{errors.lastName.message}</p>}
                    </div>
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
                    <div className="flex flex-col gap-3">
                        <label className="text-gray-700 font-medium text-lg">Confirm Password</label>
                        <input
                            type="password"
                            {...register("confirmPassword")}
                            className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 text-lg"
                        />
                        {errors.confirmPassword && <p className="text-red-500 text-sm">{errors.confirmPassword.message}</p>}
                    </div>
                    <button
                        type="submit"
                        className="w-full bg-blue-500 text-white py-3 rounded-lg hover:bg-blue-600 transition font-medium text-lg"
                    >
                        Sign Up
                    </button>
                </form>
            </div>
        </div>
    );
};

export default RegisterPage;