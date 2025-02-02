import * as Yup from "yup";
import { yupResolver } from "@hookform/resolvers/yup";
import { useForm } from "react-hook-form";
import { useAuth } from "../../Context/useAuth";
import { useState } from "react";
import { changeMyPasswordAPI } from "../../Api/apiUser";

type Props = {};

type UserProfileForm = {
    firstName: string;
    lastName: string;
    email: string;
};

type PasswordChangeForm = {
    oldPassword: string;
    newPassword: string;
    confirmPassword: string;
};

const UserProfile = (props: Props) => {
    const { user, updateUser } = useAuth();

    const userFormSchema = Yup.object().shape({
        firstName: Yup.string().required("First name is required"),
        lastName: Yup.string().required("Last name is required"),
        email: Yup.string().email("Invalid email format").required("Email is required"),
    });

    const {
        register: registerUser,
        handleSubmit: handleUserSubmit,
        formState: { errors: userErrors },
        reset: resetUserForm,
    } = useForm<UserProfileForm>({
        resolver: yupResolver(userFormSchema),
        defaultValues: {
            firstName: user.firstName,
            lastName: user.lastName,
            email: user.email,
        },
    });

    const passwordFormSchema = Yup.object().shape({
        oldPassword: Yup.string().required("Old password is required"),
        newPassword: Yup.string()
            .min(8, "Password must be at least 8 characters")
            .matches(/[A-Z]/, "Password must contain at least one uppercase letter")
            .matches(/[0-9]/, "Password must contain at least one number")
            .matches(/[@$!%*?&]/, "Password must contain at least one special character")
            .required("New password is required"),
        confirmPassword: Yup.string()
            .oneOf([Yup.ref("newPassword"), ""], "Passwords must match")
            .required("Confirm password is required"),
    });

    const {
        register: registerPassword,
        handleSubmit: handlePasswordSubmit,
        formState: { errors: passwordErrors },
        reset: resetPasswordForm,
    } = useForm<PasswordChangeForm>({
        resolver: yupResolver(passwordFormSchema),
    });

    const[isEditing, setIsEditing] = useState(false);
    const [isPasswordEditing, setIsPasswordEditing] = useState(false);

    const handleUserSave = async (form: UserProfileForm) => {
        await updateUser(form);
        setIsEditing(false);
    };

    const handlePasswordSave = async (form: PasswordChangeForm) => {
        await changeMyPasswordAPI(form);
        setIsPasswordEditing(false);
    };

    const handleCancel = () => {
        resetUserForm();
        setIsEditing(false); 
    };

    const handlePasswordCancel = () => {
        resetPasswordForm(); 
        setIsPasswordEditing(false);
    };

    return (
        <div className="w-full flex space-x-8">
            <div className="w-full max-w-xl pl-6 bg-white shadow-xl rounded-r-lg py-6 pr-6 relative">
                <h2 className="text-2xl font-bold text-blue-600 mb-6">User Profile</h2>

                {!isEditing && (
                    <button
                        onClick={() => setIsEditing(true)}
                        className="absolute top-6 right-6 px-4 py-2 bg-blue-600 text-white rounded"
                    >
                        Edit
                    </button>
                )}

                <form onSubmit={handleUserSubmit(handleUserSave)} className="space-y-4">
                    <div>
                        <p className="text-gray-500 text-sm">First Name</p>
                        {isEditing ? (
                            <input
                                type="text"
                                {...registerUser("firstName")}
                                className="text-lg font-semibold text-blue-600 border p-2 rounded"
                            />
                        ) : (
                            <p className="text-lg font-semibold text-blue-600">{user.firstName}</p>
                        )}
                        {userErrors.firstName && <p className="text-red-500">{userErrors.firstName.message}</p>}
                    </div>

                    <div>
                        <p className="text-gray-500 text-sm">Last Name</p>
                        {isEditing ? (
                            <input
                                type="text"
                                {...registerUser("lastName")}
                                className="text-lg font-semibold text-blue-600 border p-2 rounded"
                            />
                        ) : (
                            <p className="text-lg font-semibold text-blue-600">{user.lastName}</p>
                        )}
                        {userErrors.lastName && <p className="text-red-500">{userErrors.lastName.message}</p>}
                    </div>

                    <div>
                        <p className="text-gray-500 text-sm">Email</p>
                        {isEditing ? (
                            <input
                                type="email"
                                {...registerUser("email")}
                                className="text-lg font-semibold text-blue-600 border p-2 rounded"
                            />
                        ) : (
                            <p className="text-lg font-semibold text-blue-600">{user.email}</p>
                        )}
                        {userErrors.email && <p className="text-red-500">{userErrors.email.message}</p>}
                    </div>

                    {isEditing && (
                        <div className="flex justify-end space-x-4 mt-6">
                            <button
                                type="submit"
                                className="px-6 py-2 bg-blue-600 text-white rounded"
                            >
                                Save
                            </button>
                            <button
                                type="button"
                                onClick={handleCancel}
                                className="px-6 py-2 bg-red-600 text-white rounded"
                            >
                                Cancel
                            </button>
                        </div>
                    )}
                </form>
            </div>

            <div className="w-full max-w-xl pl-6 bg-white shadow-xl rounded-r-lg py-6 pr-6 relative">
                <h2 className="text-2xl font-bold text-blue-600 mb-6">Change Password</h2>

                {!isPasswordEditing && (
                    <button
                        onClick={() => setIsPasswordEditing(true)}
                        className="absolute top-6 right-6 px-4 py-2 bg-blue-600 text-white rounded"
                    >
                        Edit Password
                    </button>
                )}

                <form onSubmit={handlePasswordSubmit(handlePasswordSave)} className="space-y-4">
                    <div>
                        <p className="text-gray-500 text-sm">Old Password</p>
                        {isPasswordEditing ? (
                            <input
                                type="password"
                                {...registerPassword("oldPassword")}
                                className="text-lg font-semibold text-blue-600 border p-2 rounded"
                            />
                        ) : (
                            <p className="text-lg font-semibold text-blue-600">********</p>
                        )}
                        {passwordErrors.oldPassword && (
                            <p className="text-red-500">{passwordErrors.oldPassword.message}</p>
                        )}
                    </div>

                    {isPasswordEditing && (
                        <>
                            <div>
                                <p className="text-gray-500 text-sm">New Password</p>
                                <input
                                    type="password"
                                    {...registerPassword("newPassword")}
                                    className="text-lg font-semibold text-blue-600 border p-2 rounded"
                                />
                                {passwordErrors.newPassword && (
                                    <p className="text-red-500">{passwordErrors.newPassword.message}</p>
                                )}
                            </div>

                            <div>
                                <p className="text-gray-500 text-sm">Confirm Password</p>
                                <input
                                    type="password"
                                    {...registerPassword("confirmPassword")}
                                    className="text-lg font-semibold text-blue-600 border p-2 rounded"
                                />
                                {passwordErrors.confirmPassword && (
                                    <p className="text-red-500">{passwordErrors.confirmPassword.message}</p>
                                )}
                            </div>
                        </>
                    )}

                    {isPasswordEditing && (
                        <div className="flex justify-end space-x-4 mt-6">
                            <button
                                type="submit"
                                className="px-6 py-2 bg-blue-600 text-white rounded"
                            >
                                Save Password
                            </button>
                            <button
                                type="button"
                                onClick={handlePasswordCancel}
                                className="px-6 py-2 bg-red-600 text-white rounded"
                            >
                                Cancel
                            </button>
                        </div>
                    )}
                </form>
            </div>
        </div>
    );
};

export default UserProfile;
