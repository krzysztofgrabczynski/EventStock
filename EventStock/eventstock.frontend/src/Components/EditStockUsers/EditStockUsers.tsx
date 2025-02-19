import { useLocation } from "react-router";

interface Props { }

const EditStockUsers = (props: Props) => {
    const location = useLocation();
    const users = location.state?.users || [];

    return (
        <div>
            <h2 className="text-2xl font-bold mb-4">Edit User Roles</h2>
            <ul className="space-y-2">
                {users.map((user) => (
                    <li key={user.id} className="p-4 bg-white rounded shadow border border-gray-200">
                        <p className="text-xl font-semibold">{user.firstName} {user.lastName}</p>
                        <p className="text-sm text-gray-600">{user.email}</p>
                    </li>
                ))}
            </ul>
        </div>
    );

};

export default EditStockUsers;