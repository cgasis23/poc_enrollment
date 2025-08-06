import React, { useState } from 'react'
import CustomerLocate from './components/CustomerLocate'
import SetupAccount from './components/SetupAccount'
import ThankYou from './components/ThankYou'

function App() {
  const [currentStep, setCurrentStep] = useState(1)
  const [formData, setFormData] = useState({
    accountNumber: '',
    ssn: '',
    birthdate: '',
    email: '',
    username: '',
    password: '',
    confirmPassword: ''
  })

  const handleNext = (data) => {
    setFormData(prev => ({ ...prev, ...data }))
    setCurrentStep(prev => prev + 1)
  }

  const handleBack = () => {
    setCurrentStep(prev => prev - 1)
  }

  const renderStep = () => {
    switch (currentStep) {
      case 1:
        return <CustomerLocate formData={formData} onNext={handleNext} />
      case 2:
        return <SetupAccount formData={formData} onNext={handleNext} onBack={handleBack} />
      case 3:
        return <ThankYou />
      default:
        return <CustomerLocate formData={formData} onNext={handleNext} />
    }
  }

  return (
    <div className="min-h-screen bg-gradient-to-br from-gray-50 to-gray-100 py-8 px-4">
      <div className="max-w-4xl mx-auto">
        <div className="text-center mb-8">
          <h1 className="text-4xl font-bold text-gray-900 mb-2">Account Opening</h1>
          <div className="flex justify-center items-center space-x-4">
            <div className={`w-8 h-8 rounded-full flex items-center justify-center text-sm font-semibold ${
              currentStep >= 1 ? 'bg-primary-600 text-white' : 'bg-gray-200 text-gray-500'
            }`}>
              1
            </div>
            <div className={`w-16 h-1 ${
              currentStep >= 2 ? 'bg-primary-600' : 'bg-gray-200'
            }`}></div>
            <div className={`w-8 h-8 rounded-full flex items-center justify-center text-sm font-semibold ${
              currentStep >= 2 ? 'bg-primary-600 text-white' : 'bg-gray-200 text-gray-500'
            }`}>
              2
            </div>
            <div className={`w-16 h-1 ${
              currentStep >= 3 ? 'bg-primary-600' : 'bg-gray-200'
            }`}></div>
            <div className={`w-8 h-8 rounded-full flex items-center justify-center text-sm font-semibold ${
              currentStep >= 3 ? 'bg-primary-600 text-white' : 'bg-gray-200 text-gray-500'
            }`}>
              3
            </div>
          </div>
        </div>
        
        <div className="flex justify-center">
          {renderStep()}
        </div>
      </div>
    </div>
  )
}

export default App 