import React, { useState, useEffect } from 'react'
import mfaService from '../utils/mfaService'

function MFASetup({ userData, onComplete }) {
  const [mfaCode, setMfaCode] = useState('')
  const [mfaMethod, setMfaMethod] = useState('email') // 'email' or 'sms'
  const [isCodeSent, setIsCodeSent] = useState(false)
  const [countdown, setCountdown] = useState(0)
  const [errors, setErrors] = useState({})
  const [isLoading, setIsLoading] = useState(false)

  // Send MFA code
  const sendMFACode = async () => {
    setIsLoading(true)
    try {
      const identifier = mfaMethod === 'email' ? userData.email : userData.phoneNumber
      const result = mfaMethod === 'email' 
        ? await mfaService.sendEmailCode(identifier)
        : await mfaService.sendSMSCode(identifier)
      
      if (result.success) {
        setIsCodeSent(true)
        setCountdown(30) // 30 second countdown
        setErrors({})
      } else {
        setErrors({ general: result.message })
      }
    } catch (error) {
      setErrors({ general: 'Failed to send code. Please try again.' })
    } finally {
      setIsLoading(false)
    }
  }

  // Countdown timer
  useEffect(() => {
    if (countdown > 0) {
      const timer = setTimeout(() => setCountdown(countdown - 1), 1000)
      return () => clearTimeout(timer)
    }
  }, [countdown])

  const handleInputChange = (e) => {
    const { value } = e.target
    // Only allow numbers and limit to 6 digits
    if (/^\d{0,6}$/.test(value)) {
      setMfaCode(value)
      if (errors.code) {
        setErrors(prev => ({ ...prev, code: '' }))
      }
    }
  }

  const validateForm = () => {
    const newErrors = {}
    
    if (!mfaCode) {
      newErrors.code = 'Please enter the verification code'
    } else if (mfaCode.length !== 6) {
      newErrors.code = 'Please enter the complete 6-digit code'
    }
    
    setErrors(newErrors)
    return Object.keys(newErrors).length === 0
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    if (validateForm()) {
      setIsLoading(true)
      try {
        const identifier = mfaMethod === 'email' ? userData.email : userData.phoneNumber
        const result = await mfaService.verifyCode(identifier, mfaCode)
        
        if (result.success) {
          onComplete({ mfaVerified: true, mfaMethod })
        } else {
          setErrors({ code: result.message })
        }
      } catch (error) {
        setErrors({ code: 'Failed to verify code. Please try again.' })
      } finally {
        setIsLoading(false)
      }
    }
  }

  const handleResendCode = () => {
    sendMFACode()
  }

  const handleSkipMFA = () => {
    onComplete({ mfaVerified: false, mfaMethod: null })
  }

  return (
    <div className="min-h-screen bg-gradient-to-br from-gray-50 to-gray-100 py-8 px-4">
      <div className="max-w-4xl mx-auto">
        <div className="text-center mb-8">
          <h1 className="text-4xl font-bold text-gray-900 mb-2">Two-Factor Authentication Setup</h1>
          <p className="text-gray-600">Enhance your account security with two-factor authentication</p>
        </div>
        
        <div className="flex justify-center">
          <div className="card w-full max-w-md">
            <div className="text-center mb-6">
              <div className="w-16 h-16 bg-blue-100 rounded-full flex items-center justify-center mx-auto mb-4">
                <svg className="w-8 h-8 text-blue-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2zm10-10V7a4 4 0 00-8 0v4h8z" />
                </svg>
              </div>
              <h2 className="text-2xl font-bold text-gray-900 mb-2">Secure Your Account</h2>
              <p className="text-gray-600">Choose your preferred verification method</p>
            </div>

            {!isCodeSent ? (
              <div className="space-y-4">
                <div className="bg-blue-50 rounded-lg p-4 mb-4">
                  <p className="text-sm text-blue-800">
                    Two-factor authentication adds an extra layer of security to your account. 
                    You'll need to enter a verification code in addition to your password when signing in.
                  </p>
                </div>

                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-3">
                    Choose verification method:
                  </label>
                  <div className="space-y-3">
                    <label className="flex items-center space-x-3 cursor-pointer">
                      <input
                        type="radio"
                        name="mfaMethod"
                        value="email"
                        checked={mfaMethod === 'email'}
                        onChange={(e) => setMfaMethod(e.target.value)}
                        className="text-blue-600"
                      />
                      <span className="text-sm text-gray-700">
                        Email verification (to {userData.email})
                      </span>
                    </label>
                    {userData.phoneNumber && (
                      <label className="flex items-center space-x-3 cursor-pointer">
                        <input
                          type="radio"
                          name="mfaMethod"
                          value="sms"
                          checked={mfaMethod === 'sms'}
                          onChange={(e) => setMfaMethod(e.target.value)}
                          className="text-blue-600"
                        />
                        <span className="text-sm text-gray-700">
                          SMS verification (to {userData.phoneNumber})
                        </span>
                      </label>
                    )}
                  </div>
                </div>

                {errors.general && (
                  <p className="text-red-500 text-sm mt-2">{errors.general}</p>
                )}
                
                <button
                  onClick={sendMFACode}
                  disabled={isLoading}
                  className="w-full btn-primary disabled:opacity-50 disabled:cursor-not-allowed"
                >
                  {isLoading ? 'Sending...' : 'Send Verification Code'}
                </button>

                <div className="text-center">
                  <button
                    onClick={handleSkipMFA}
                    className="text-sm text-gray-500 hover:text-gray-700 underline"
                  >
                    Skip for now (you can set this up later)
                  </button>
                </div>
              </div>
            ) : (
              <form onSubmit={handleSubmit} className="space-y-4">
                <div className="bg-blue-50 rounded-lg p-4 mb-4">
                  <p className="text-sm text-blue-800">
                    We've sent a 6-digit verification code to your {mfaMethod === 'email' ? 'email' : 'phone'}.
                    Please enter it below.
                  </p>
                </div>

                <div>
                  <label htmlFor="mfaCode" className="block text-sm font-medium text-gray-700 mb-2">
                    Verification Code
                  </label>
                  <input
                    type="text"
                    id="mfaCode"
                    value={mfaCode}
                    onChange={handleInputChange}
                    placeholder="Enter 6-digit code"
                    className={`form-input text-center text-lg tracking-widest ${errors.code ? 'border-red-500' : ''}`}
                    maxLength={6}
                    autoComplete="one-time-code"
                  />
                  {errors.code && (
                    <p className="text-red-500 text-sm mt-1">{errors.code}</p>
                  )}
                </div>

                <div className="text-center">
                  {countdown > 0 ? (
                    <p className="text-sm text-gray-500">
                      Resend code in {countdown} seconds
                    </p>
                  ) : (
                    <button
                      type="button"
                      onClick={handleResendCode}
                      className="text-sm text-blue-600 hover:text-blue-800 underline"
                    >
                      Resend code
                    </button>
                  )}
                </div>

                <div className="flex space-x-4 mt-6">
                  <button
                    type="button"
                    onClick={() => setIsCodeSent(false)}
                    className="flex-1 bg-gray-200 hover:bg-gray-300 text-gray-700 font-semibold py-3 px-6 rounded-lg transition-all duration-200"
                  >
                    Back
                  </button>
                  <button 
                    type="submit" 
                    disabled={isLoading}
                    className="flex-1 btn-primary disabled:opacity-50 disabled:cursor-not-allowed"
                  >
                    {isLoading ? 'Verifying...' : 'Verify & Complete'}
                  </button>
                </div>
              </form>
            )}
          </div>
        </div>
      </div>
    </div>
  )
}

export default MFASetup 