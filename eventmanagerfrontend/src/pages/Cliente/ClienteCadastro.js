import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import './ClienteCadastro.css';

const ClienteCadastro = () => {
    const [cliente, setCliente] = useState({
        usuario: '',
        nome: '',
        senha: '',
        email: '',
        telefone: '',
        instagram: ''
    });

    const navigate = useNavigate();

    const handleChange = (e) => {
        const { name, value } = e.target;
        setCliente({ ...cliente, [name]: value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        const clienteData = {
            ...cliente,
            email: cliente.email === "" ? null : cliente.email,
            telefone: cliente.telefone === "" ? null : cliente.telefone,
            instagram: cliente.instagram === "" ? null : cliente.instagram
        };

        try {
            const response = await axios.post(`${process.env.REACT_APP_API_URL}/Cliente`, clienteData);
            alert("Cliente cadastrado com sucesso!");
            navigate("/login");
        } catch(error) {
            console.error('Erro ao cadastrar cliente:', error.response || error.message || error);
            alert("Erro ao cadastrar cliente.");
        }
    };

    return (
        <div className="cliente-cadastro-container">
            <h1>Cadastro de cliente</h1>
            <form onSubmit={handleSubmit}>
                <label htmlFor="usuario">Usuário:</label>                    
                <input type="text" id="usuario" name="usuario" value={cliente.usuario} onChange={handleChange} required/>                      
                <label htmlFor="senha">Senha:</label>                     
                <input type="password" id="senha" name="senha" value={cliente.senha} onChange={handleChange} required/>                     
                <label htmlFor="nome">Nome:</label>  
                <input type="text" id="nome" name="nome" value={cliente.nome} onChange={handleChange} required/>                      
                <label htmlFor="email">Email:</label>
                <input type="email" id="email" name="email" value={cliente.email} onChange={handleChange}/>                     
                <label htmlFor="telefone">Telefone:</label> 
                <input type="text" id="telefone" name="telefone" value={cliente.telefone} onChange={handleChange}/>                    
                <label htmlFor="instagram">Instagram:</label> 
                <input type="text" id="instagram" name="instagram" value={cliente.instagram} onChange={handleChange}/>                    
                <button type="submit">Cadastrar</button>
            </form>    
        </div>
    );

};
export default ClienteCadastro;