import { Navigate } from 'react-router-dom';

const ProtectedRoute = ({ children }) => {
    const cliente = JSON.parse(localStorage.getItem('cliente'));
    
    return cliente ? children : <Navigate to="/login"/>
};

export default ProtectedRoute;