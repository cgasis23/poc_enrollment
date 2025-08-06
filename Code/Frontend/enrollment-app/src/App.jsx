import React, { useState } from 'react'
import CustomerLocate from './components/CustomerLocate'
import SetupAccount from './components/SetupAccount'
import ThankYou from './components/ThankYou'
import MFADemo from './components/MFADemo'

function App() {
  const [currentStep, setCurrentStep] = useState(1)
  const [showMFADemo, setShowMFADemo] = useState(false)
  const [formData, setFormData] = useState({
    accountNumber: '',
    ssn: '',
    birthdate: '',
    email: '',
    username: '',
    password: '',
    confirmPassword: '',
    phoneNumber: ''
  })

  const handleNext = (data) => {
    setFormData(prev => ({ ...prev, ...data }))
    setCurrentStep(prev => prev + 1)
  }

  const handleBack = () => {
    setCurrentStep(prev => prev - 1)
  }

  const handleShowMFADemo = () => {
    setShowMFADemo(true)
  }

  const handleBackToEnrollment = () => {
    setShowMFADemo(false)
    setCurrentStep(1)
    setFormData({
      accountNumber: '',
      ssn: '',
      birthdate: '',
      email: '',
      username: '',
      password: '',
      confirmPassword: '',
      phoneNumber: ''
    })
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

  if (showMFADemo) {
    return (
      <div>
        <MFADemo />
        <div className="fixed top-4 left-4">
          <button
            onClick={handleBackToEnrollment}
            className="bg-gray-200 hover:bg-gray-300 text-gray-700 font-semibold py-2 px-4 rounded-lg transition-all duration-200"
          >
            ← Back to Enrollment
          </button>
        </div>
      </div>
    )
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
        
        {/* Demo button */}
        <div className="fixed bottom-4 left-4">
          <button
            onClick={handleShowMFADemo}
            className="bg-blue-600 hover:bg-blue-700 text-white font-semibold py-2 px-4 rounded-lg transition-all duration-200"
          >
            🛡️ Demo MFA Setup
          </button>
        </div>
      </div>
    </div>
  )
}

export default App 