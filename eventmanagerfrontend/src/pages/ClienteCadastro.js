import React, { useState } from 'react';
import axios from 'axios';

const ClienteCadastro = () => {
    const [cliente, setCliente] = useState({
        usuario: '',
        nome: '',
        senha: '',
        email: '',
        telefone: '',
        instagram: ''
})};

const handleChange = (e) => {
    const { name, value } = e.target;
    setCliente({ ...cliente, [name]: value });
};

const handleSubmit = async (e) => {
    e.preventDefault();
    try {
        const response = await axios.post(`${process.env.development.local.REACT_APP_API_URL}/Cliente`, cliente);
        alert('Cliente cadastrado com sucesso!');
    } catch(error) {
        console.error(error);
        alert('Erro ao cadastrar cliente.');
    }
};

return (
    <div>
        <h2>Cadastro de cliente</h2>
        <form>
            <label>
                Usuário:
                <input type="text" name="usuario" value="(cliente.usuario)" onchange={handleChange} required/>    
            </label>  
            <label>
                Senha:
                <input type="password" name="senha" value="(cliente.senha)" onchange={handleChange} required/>    
            </label>  
            <label>
                Nome:
                <input type="text" name="nome" value="(cliente.nome)" onchange={handleChange} required/>    
            </label>    
            <label>
                Email:
                <input type="email" name="email" value="(cliente.email)" onchange={handleChange}/>    
            </label> 
            <label>
                Telefone:
                <input type="text" name="telefone" value="(cliente.telefone)" onchange={handleChange}/>    
            </label> 
            <label>
                Instagram:
                <input type="text" name="instagram" value="(cliente.instagram)" onchange={handleChange}/>    
            </label> 
            <button type="submit">Cadastrar</button>
        </form>    
    </div>
)

export default ClienteCadastro;