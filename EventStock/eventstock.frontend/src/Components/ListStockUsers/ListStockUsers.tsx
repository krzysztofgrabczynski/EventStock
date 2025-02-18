import { useEffect, useState } from "react";
import { listUserInStockAPI } from "../../Api/apiStock";
import { useAuth } from "../../Context/useAuth";
import { UserForListUsers } from "../../Models/User/UserForListUsers";

interface Props { }

const ListStockUsers = (props: Props) => {
    const { user } = useAuth()
    const [users, setUsers] = useState<UserForListUsers[]>([]);

    useEffect(() => {
        const fetchUsers = async () => {
            const response = await listUserInStockAPI(user.stockId);
            setUsers(response.data);
        };

        if (user.stockId) {
            fetchUsers();
        }
    }, [user.stockId]);

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
                        </li>
                    ))}
                </ul>
            )}
        </div>

    );
};

export default ListStockUsers;