import React, { useState } from 'react';
import axios from 'axios';
import InputMask from 'react-input-mask';
import { useNavigate } from 'react-router-dom';
import './ClienteCadastro.css';

const ClienteCadastro = () => {
    const [cliente, setCliente] = useState({
        usuario: '',
        nome: '',
        senha: '',
        email: '',
        telefone: '',
        instagram: '@'
    });

    const navigate = useNavigate();

    const handleChange = (e) => {
        const { name, value } = e.target;

        if(name === 'usuario' && value.length > 15) return;
        if(name === 'senha' && value.length > 60) return;
        if(name === 'nome' && value.length > 100) return;
        if(name === 'email' && value.length > 100) return;
        if(name === 'telefone' && value.length > 15) return;
        if(name === 'instagram' && value.length > 50) return;

        setCliente({ ...cliente, [name]: value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        const clienteData = {
            ...cliente,
            email: cliente.email === "" ? null : cliente.email,
            telefone: cliente.telefone === "" ? null : cliente.telefone.replace(/\D/g, ""),
            instagram: cliente.instagram === "@" ? null : cliente.instagram
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
                <input type="text" id="usuario" name="usuario" value={cliente.usuario} onChange={handleChange} maxLength={15} required/>                      
                <label htmlFor="senha">Senha:</label>                     
                <input type="password" id="senha" name="senha" value={cliente.senha} onChange={handleChange} maxLength={60} required/>                     
                <label htmlFor="nome">Nome:</label>  
                <input type="text" id="nome" name="nome" value={cliente.nome} onChange={handleChange} maxLength={100} required/>                      
                <label htmlFor="email">Email:</label>
                <input type="email" id="email" name="email" value={cliente.email} onChange={handleChange} maxLength={100}/>                     
                <label htmlFor="telefone">Telefone:</label> 
                <InputMask mask="(99) 99999-9999" maskChar={null} id="telefone" name="telefone" value={cliente.telefone} onChange={handleChange} maxLength={15}/>                    
                <label htmlFor="instagram">Instagram:</label> 
                <input type="text" id="instagram" name="instagram" value={cliente.instagram} onChange={handleChange} maxLength={50}/>                    
                <button type="submit">Cadastrar</button>
            </form>    
        </div>
    );

};
export default ClienteCadastro;