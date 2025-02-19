import { Link } from "react-router";
import logo from "./logo.svg";
import { useAuth } from "../../Context/useAuth";

interface Props {}

const Navbar = (props: Props) => {
    const { isLoggedIn, logoutUser, user } = useAuth();
    return (
        <nav className="relative container mx-auto p-6">
            <div className="flex items-center justify-between">
                <div className="flex items-center space-x-10">
                    <Link to="/">
                        <img src={logo} alt="logo" className="h-10" />
                    </Link>

                    {/* Jeœli u¿ytkownik jest zalogowany, poka¿ przycisk "Home" */}
                    {isLoggedIn() && (
                        <Link
                            to="/home/profile"
                            className="px-4 py-2 font-medium text-white bg-blue-500 rounded hover:bg-blue-600 transition"
                        >
                            Profile
                        </Link>
                    )}
                </div>

                {isLoggedIn() ? (
                    <div className="hidden lg:flex items-center space-x-6 text-black">
                        <span className="hover:text-darkBlue">Welcome, {user?.firstName}</span>
                        <button
                            onClick={logoutUser}
                            className="px-8 py-3 font-bold rounded text-white bg-blue-500 hover:bg-blue-600 transition"
                        >
                            Logout
                        </button>
                    </div>
                ) : (
                    <div className="hidden lg:flex items-center space-x-6 text-black">
                        <Link to="/login" className="hover:text-darkBlue text-lg font-medium">
                            Login
                        </Link>
                        <Link
                            to="/register"
                            className="px-8 py-3 font-bold rounded text-white bg-blue-500 hover:bg-blue-600 transition"
                        >
                            Signup
                        </Link>
                    </div>
                )}
            </div>
        </nav>

    );
};

export default Navbar;