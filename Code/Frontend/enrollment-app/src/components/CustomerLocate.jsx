import React, { useState } from 'react'

function CustomerLocate({ formData, onNext }) {
  const [formState, setFormState] = useState({
    accountNumber: formData.accountNumber || '',
    ssn: formData.ssn || '',
    birthdate: formData.birthdate || ''
  })

  const handleInputChange = (e) => {
    const { name, value } = e.target
    setFormState(prev => ({
      ...prev,
      [name]: value
    }))
  }

  const handleSubmit = (e) => {
    e.preventDefault()
    onNext(formState)
  }

  return (
    <div className="card w-full max-w-md">
      <div className="text-center mb-6">
        <h2 className="text-2xl font-bold text-gray-900 mb-2">Account Opening</h2>
        <p className="text-gray-600">Customer Locate</p>
      </div>
      
      <form onSubmit={handleSubmit} className="space-y-4">
        <div>
          <label htmlFor="accountNumber" className="block text-sm font-medium text-gray-700 mb-2">
            Account Number
          </label>
          <input
            type="text"
            id="accountNumber"
            name="accountNumber"
            value={formState.accountNumber}
            onChange={handleInputChange}
            placeholder="Enter account number"
            className="form-input"
            required
          />
        </div>
        
        <div>
          <label htmlFor="ssn" className="block text-sm font-medium text-gray-700 mb-2">
            SSN
          </label>
          <input
            type="text"
            id="ssn"
            name="ssn"
            value={formState.ssn}
            onChange={handleInputChange}
            placeholder="Enter SSN"
            className="form-input"
            required
          />
        </div>
        
        <div>
          <label htmlFor="birthdate" className="block text-sm font-medium text-gray-700 mb-2">
            Birthdate
          </label>
          <input
            type="date"
            id="birthdate"
            name="birthdate"
            value={formState.birthdate}
            onChange={handleInputChange}
            className="form-input"
            required
          />
        </div>
        
        <button type="submit" className="btn-primary mt-6">
          Next
        </button>
      </form>
    </div>
  )
}

export default CustomerLocate 