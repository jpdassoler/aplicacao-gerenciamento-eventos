import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import ClienteCadastro from './pages/Cliente/ClienteCadastro';
import EventoCadastro from './pages/Evento/EventoCadastro';
import Login from './pages/Login/Login';
import ProtectedRoute from './routes/ProtectedRoute';
import Home from './pages/Home/Home';
import EventoDetalhes from './pages/Evento/EventoDetalhes';

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/login" element={<Login/>}/>
        <Route path="/cadastro-cliente" element={<ClienteCadastro/>}/>
        <Route path="/cadastro-evento" element={
          <ProtectedRoute>
            <EventoCadastro/>
          </ProtectedRoute>          
          }
        /> 
        <Route path="/home" element={
          <ProtectedRoute>
            <Home/>
          </ProtectedRoute>
          }
        />   
        <Route path="/evento/:id" element={
          <ProtectedRoute>
            <EventoDetalhes/>
          </ProtectedRoute>
        }/>
        <Route path="/" element={<Navigate to="/home" />} />
      </Routes>
    </Router>
  );
}

export default App;
