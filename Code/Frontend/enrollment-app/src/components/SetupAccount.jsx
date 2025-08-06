import React, { useState } from 'react'

function SetupAccount({ formData, onNext, onBack }) {
  const [formState, setFormState] = useState({
    email: formData.email || '',
    username: formData.username || '',
    password: formData.password || '',
    confirmPassword: formData.confirmPassword || '',
    phoneNumber: formData.phoneNumber || ''
  })
  const [errors, setErrors] = useState({})

  const handleInputChange = (e) => {
    const { name, value } = e.target
    setFormState(prev => ({
      ...prev,
      [name]: value
    }))
    // Clear error when user starts typing
    if (errors[name]) {
      setErrors(prev => ({
        ...prev,
        [name]: ''
      }))
    }
  }

  const validateForm = () => {
    const newErrors = {}
    
    if (!formState.email) {
      newErrors.email = 'Email is required'
    } else if (!/\S+@\S+\.\S+/.test(formState.email)) {
      newErrors.email = 'Please enter a valid email'
    }
    
    if (!formState.username) {
      newErrors.username = 'Username is required'
    } else if (formState.username.length < 3) {
      newErrors.username = 'Username must be at least 3 characters'
    }
    
    if (!formState.password) {
      newErrors.password = 'Password is required'
    } else if (formState.password.length < 6) {
      newErrors.password = 'Password must be at least 6 characters'
    }
    
    if (!formState.confirmPassword) {
      newErrors.confirmPassword = 'Please confirm your password'
    } else if (formState.password !== formState.confirmPassword) {
      newErrors.confirmPassword = 'Passwords do not match'
    }
    
    if (formState.phoneNumber && !/^\+?[\d\s\-\(\)]{10,}$/.test(formState.phoneNumber)) {
      newErrors.phoneNumber = 'Please enter a valid phone number'
    }
    
    setErrors(newErrors)
    return Object.keys(newErrors).length === 0
  }

  const handleSubmit = (e) => {
    e.preventDefault()
    if (validateForm()) {
      onNext(formState)
    }
  }

  return (
    <div className="card w-full max-w-md">
      <div className="text-center mb-6">
        <h2 className="text-2xl font-bold text-gray-900 mb-2">Account Opening</h2>
        <p className="text-gray-600">Setup Account</p>
      </div>
      
      <form onSubmit={handleSubmit} className="space-y-4">
        <div>
          <label htmlFor="email" className="block text-sm font-medium text-gray-700 mb-2">
            Email
          </label>
          <input
            type="email"
            id="email"
            name="email"
            value={formState.email}
            onChange={handleInputChange}
            placeholder="Enter your email"
            className={`form-input ${errors.email ? 'border-red-500' : ''}`}
          />
          {errors.email && (
            <p className="text-red-500 text-sm mt-1">{errors.email}</p>
          )}
        </div>
        
        <div>
          <label htmlFor="username" className="block text-sm font-medium text-gray-700 mb-2">
            Username
          </label>
          <input
            type="text"
            id="username"
            name="username"
            value={formState.username}
            onChange={handleInputChange}
            placeholder="Choose a username"
            className={`form-input ${errors.username ? 'border-red-500' : ''}`}
          />
          {errors.username && (
            <p className="text-red-500 text-sm mt-1">{errors.username}</p>
          )}
        </div>
        
        <div>
          <label htmlFor="password" className="block text-sm font-medium text-gray-700 mb-2">
            Password
          </label>
          <input
            type="password"
            id="password"
            name="password"
            value={formState.password}
            onChange={handleInputChange}
            placeholder="Create a password"
            className={`form-input ${errors.password ? 'border-red-500' : ''}`}
          />
          {errors.password && (
            <p className="text-red-500 text-sm mt-1">{errors.password}</p>
          )}
        </div>
        
        <div>
          <label htmlFor="confirmPassword" className="block text-sm font-medium text-gray-700 mb-2">
            Confirm Password
          </label>
          <input
            type="password"
            id="confirmPassword"
            name="confirmPassword"
            value={formState.confirmPassword}
            onChange={handleInputChange}
            placeholder="Confirm your password"
            className={`form-input ${errors.confirmPassword ? 'border-red-500' : ''}`}
          />
          {errors.confirmPassword && (
            <p className="text-red-500 text-sm mt-1">{errors.confirmPassword}</p>
          )}
        </div>
        
        <div>
          <label htmlFor="phoneNumber" className="block text-sm font-medium text-gray-700 mb-2">
            Phone Number
          </label>
          <input
            type="tel"
            id="phoneNumber"
            name="phoneNumber"
            value={formState.phoneNumber}
            onChange={handleInputChange}
            placeholder="Enter your phone number"
            className={`form-input ${errors.phoneNumber ? 'border-red-500' : ''}`}
          />
          <p className="text-xs text-gray-500 mt-1">
            Optional - can be used for two-factor authentication setup later
          </p>
          {errors.phoneNumber && (
            <p className="text-red-500 text-sm mt-1">{errors.phoneNumber}</p>
          )}
        </div>
        
        <div className="flex space-x-4 mt-6">
          <button
            type="button"
            onClick={onBack}
            className="flex-1 bg-gray-200 hover:bg-gray-300 text-gray-700 font-semibold py-3 px-6 rounded-lg transition-all duration-200"
          >
            Back
          </button>
          <button type="submit" className="flex-1 btn-primary">
            Submit
          </button>
        </div>
      </form>
    </div>
  )
}

export default SetupAccount 