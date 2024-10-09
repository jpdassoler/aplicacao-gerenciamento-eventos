import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import './Login.css';

const Login = () => {
    const [usuario, setUsuario] = useState('');
    const [senha, setSenha] = useState('');
    const navigate = useNavigate();

    const handleLogin = async (e) => {
        e.preventDefault();
        try {
            const response = await axios.post(`${process.env.REACT_APP_API_URL}/Cliente/login`, {
                usuario: usuario,
                senha: senha,
            });

            if (response.data.success) {
                //Armazena os dados do usuário para uso posterior
                localStorage.setItem('cliente', JSON.stringify(response.data.cliente));

                //Redireciona para a página principal após o login
                navigate('/home');
            } else {
                alert("Usuário ou senha incorretos.")
            }
        } catch(error) {
            console.error('Erro ao tentar fazer login:', error.response || error.message || error);
            alert("Erro ao tentar fazer login.");
        }
    };

    return (
        <div className="login-container">
            <h1>Gerenciador de Eventos</h1>
            <h2>Login</h2>
            <form onSubmit={handleLogin}>
                <label htmlFor="usuario">Usuário:</label>
                <input type="text" id="usuario" name="usuario" value={usuario} onChange={(e) => setUsuario(e.target.value)} required/>
                <label htmlFor="senha">Senha:</label>
                <input type="password" id="senha" name="senha" value={senha} onChange={(e) => setSenha(e.target.value)} required/>
                <button type="submit">Entrar</button>
            </form>
            <p>
                Não tem conta? <a href="/cadastro-cliente">Crie uma conta</a>
            </p>
        </div>
    );
};

export default Login;