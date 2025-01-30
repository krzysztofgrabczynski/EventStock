import './App.css'
import { Outlet } from 'react-router';
import { AuthProvider } from './Context/useAuth';
import Navbar from './Components/Navbar/Navbar';

function App() {
    return (
        <>
            <AuthProvider>
                <Navbar />
                <Outlet />
            </AuthProvider>
        </>
    );
};

export default App;