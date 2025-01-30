import { Link } from "react-router-dom";
import userIcon from "./userIcon.png";

interface Props { }

const Sidebar = (props: Props) => {
    return (
        <nav className="fixed top-30 left-0 h-full w-64 bg-white shadow-xl flex flex-col p-6 z-50">
            <div className="flex items-center space-x-4 pb-6 border-b">
                <img src={userIcon} alt="User icon" className="w-12 h-12 rounded-full"/>
                <div>
                    <h2 className="text-lg font-bold">John Doe</h2>
                    <p className="text-sm text-gray-500">johndoe@example.com</p>
                </div>
            </div>

            <ul className="mt-6 space-y-4">
                <li>
                    <Link
                        to="profile">
                        <a href="#" className="block text-lg font-medium text-gray-700 hover:text-blue-500">Profile</a>
                    </Link>
                </li>
                <li>
                    <a href="#" className="block text-lg font-medium text-gray-700 hover:text-blue-500">Settings</a>
                </li>
                <li>
                    <a href="#" className="block text-lg font-medium text-gray-700 hover:text-blue-500">Logout</a>
                </li>
            </ul>
        </nav>
    );
};

export default Sidebar;