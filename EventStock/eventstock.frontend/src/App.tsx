import './App.css'
import { Outlet } from 'react-router';
import { AuthProvider } from './Context/useAuth';

function App() {
    return (
        <>
            <AuthProvider>
                <Outlet />
            </AuthProvider>
        </>
    );
};

export default App;