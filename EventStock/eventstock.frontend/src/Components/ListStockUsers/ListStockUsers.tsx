import { useEffect, useState } from "react";
import { listUserInStockAPI, editUsersRoleInStockAPI, deleteUserFromStockAPI } from "../../Api/apiStock";
import { useAuth } from "../../Context/useAuth";
import { UserForListUsers } from "../../Models/User/UserForListUsers";
import { Roles } from "../../Models/Stock/Roles";
import { UpdateUserRole } from "../../Models/Stock/UpdateUserRole";
import { DeleteUserFromStock } from "../../Models/Stock/DeleteUserFromStock";

interface Props { }

const ListStockUsers = (props: Props) => {
    const { user } = useAuth();
    const [users, setUsers] = useState<UserForListUsers[]>([]);
    const [selectedUser, setSelectedUser] = useState<UserForListUsers | null>(null);
    const [newRole, setNewRole] = useState<string>("");
    const [showModal, setShowModal] = useState(false);
    const [showDeleteModal, setShowDeleteModal] = useState(false); // Dodanie zmiennej do kontrolowania modala usuwania u¿ytkownika

    useEffect(() => {
        const fetchUsers = async () => {
            const response = await listUserInStockAPI(user.stockId);
            setUsers(response.data);
        };

        if (user.stockId) {
            fetchUsers();
        }
    }, [user.stockId]);

    const hasEditPermissions = user.roles.includes("StockAdmin") || user.roles.includes("StockModerator");

    const handleEditClick = (userToEdit: UserForListUsers) => {
        setSelectedUser(userToEdit);
        setNewRole(userToEdit.roles[0] || "");
        setShowModal(true);
    };

    const handleSaveRole = async () => {
        if (selectedUser) {
            const updateUserRoleRequest: UpdateUserRole = {
                stockId: user.stockId,
                email: selectedUser.email,
                role: Roles[newRole as keyof typeof Roles],
            };

            await editUsersRoleInStockAPI(updateUserRoleRequest);

            const updatedUsers = users.map((u) =>
                u.id === selectedUser.id ? { ...u, roles: [newRole] } : u
            );
            setUsers(updatedUsers);

            setShowModal(false);
            setSelectedUser(null);
        }
    };

    const handleDeleteClick = (userToDelete: UserForListUsers) => {
        setSelectedUser(userToDelete);
        setShowDeleteModal(true); // Pokazuje modal do potwierdzenia usuniêcia
    };

    const handleDeleteUser = async () => {
        if (selectedUser) {
            const deleteUserRequest: DeleteUserFromStock = {
                stockId: user.stockId,
                userId: selectedUser.id
            };
            await deleteUserFromStockAPI(deleteUserRequest);

            setUsers(users.filter((u) => u.id !== selectedUser.id)); // Usuwa u¿ytkownika z listy

            setShowDeleteModal(false);
            setSelectedUser(null);
        }
    };

    return (
        <div>
            <h2 className="text-2xl font-bold mb-4 text-blue-600">List of Users in Stock</h2>

            {users.length === 0 ? (
                <p className="text-gray-600">No users found for this stock.</p>
            ) : (
                <ul className="space-y-2">
                    {users.map((user) => (
                        <li key={user.id} className="p-4 bg-white rounded shadow border border-gray-200">
                            <p className="text-xl font-semibold text-blue-600">
                                {user.firstName} {user.lastName}
                            </p>
                            <p className="text-sm text-gray-600">{user.email}</p>
                            {user.roles && (
                                <p className="text-sm text-gray-500">
                                    Roles: <span className="font-bold">{user.roles.join(", ")}</span>
                                </p>
                            )}

                            {hasEditPermissions && (
                                <div className="flex space-x-2 mt-2">
                                    <button
                                        className="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-700"
                                        onClick={() => handleEditClick(user)}
                                    >
                                        Edit Role
                                    </button>
                                    <button
                                        className="px-4 py-2 bg-red-500 text-white rounded hover:bg-red-700"
                                        onClick={() => handleDeleteClick(user)}
                                    >
                                        Delete User
                                    </button>
                                </div>
                            )}
                        </li>
                    ))}
                </ul>
            )}

            {showModal && selectedUser && (
                <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50">
                    <div className="bg-white p-6 rounded shadow-lg w-96">
                        <h3 className="text-xl font-bold mb-4">
                            Edit Role for {selectedUser.firstName} {selectedUser.lastName}
                        </h3>

                        <label className="block mb-2 text-gray-700">Select New Role:</label>
                        <select
                            className="w-full p-2 border border-gray-300 rounded"
                            value={newRole}
                            onChange={(e) => setNewRole(e.target.value)}
                        >
                            {Object.keys(Roles)
                                .filter((key) => isNaN(Number(key)))
                                .map((roleKey) => (
                                    <option key={roleKey} value={roleKey}>
                                        {roleKey}
                                    </option>
                                ))}
                        </select>

                        <div className="flex justify-end mt-4">
                            <button
                                className="px-4 py-2 mr-2 bg-gray-300 rounded hover:bg-gray-400"
                                onClick={() => setShowModal(false)}
                            >
                                Cancel
                            </button>
                            <button
                                className="px-4 py-2 bg-green-500 text-white rounded hover:bg-green-700"
                                onClick={handleSaveRole}
                            >
                                Save
                            </button>
                        </div>
                    </div>
                </div>
            )}

            {showDeleteModal && selectedUser && (
                <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50">
                    <div className="bg-white p-6 rounded shadow-lg w-96">
                        <h3 className="text-xl font-bold mb-4">
                            Confirm Deletion
                        </h3>
                        <p className="text-gray-700 mb-4">
                            Are you sure you want to delete {selectedUser.firstName} {selectedUser.lastName}?
                        </p>
                        <div className="flex justify-end">
                            <button
                                className="px-4 py-2 mr-2 bg-gray-300 rounded hover:bg-gray-400"
                                onClick={() => setShowDeleteModal(false)}
                            >
                                Cancel
                            </button>
                            <button
                                className="px-4 py-2 bg-red-500 text-white rounded hover:bg-red-700"
                                onClick={handleDeleteUser}
                            >
                                Delete
                            </button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default ListStockUsers;
