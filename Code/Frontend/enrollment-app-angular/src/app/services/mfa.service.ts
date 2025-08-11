import { Injectable } from '@angular/core';

interface MFAStoredData {
  code: string;
  timestamp: number;
  attempts: number;
}

interface MFAResponse {
  success: boolean;
  message: string;
}

@Injectable({
  providedIn: 'root'
})
export class MFAService {
  private storedCodes = new Map<string, MFAStoredData>();

  constructor() { }

  // Generate a random 6-digit code
  private generateCode(): string {
    return Math.floor(100000 + Math.random() * 900000).toString();
  }

  // Send MFA code via email (mock)
  async sendEmailCode(email: string): Promise<MFAResponse> {
    const code = this.generateCode();
    this.storedCodes.set(email, {
      code,
      timestamp: Date.now(),
      attempts: 0
    });
    
    console.log(`[MOCK] Email sent to ${email} with code: ${code}`);
    
    // Simulate API delay
    await new Promise(resolve => setTimeout(resolve, 1000));
    
    return { success: true, message: 'Code sent to your email' };
  }

  // Send MFA code via SMS (mock)
  async sendSMSCode(phoneNumber: string): Promise<MFAResponse> {
    const code = this.generateCode();
    this.storedCodes.set(phoneNumber, {
      code,
      timestamp: Date.now(),
      attempts: 0
    });
    
    console.log(`[MOCK] SMS sent to ${phoneNumber} with code: ${code}`);
    
    // Simulate API delay
    await new Promise(resolve => setTimeout(resolve, 1000));
    
    return { success: true, message: 'Code sent to your phone' };
  }

  // Verify MFA code
  async verifyCode(identifier: string, code: string): Promise<MFAResponse> {
    const stored = this.storedCodes.get(identifier);
    
    if (!stored) {
      return { success: false, message: 'No code found. Please request a new code.' };
    }

    // Check if code is expired (5 minutes)
    const now = Date.now();
    const fiveMinutes = 5 * 60 * 1000;
    if (now - stored.timestamp > fiveMinutes) {
      this.storedCodes.delete(identifier);
      return { success: false, message: 'Code has expired. Please request a new code.' };
    }

    // Check attempts (max 3)
    if (stored.attempts >= 3) {
      this.storedCodes.delete(identifier);
      return { success: false, message: 'Too many attempts. Please request a new code.' };
    }

    stored.attempts++;

    if (stored.code === code) {
      this.storedCodes.delete(identifier);
      return { success: true, message: 'Code verified successfully' };
    } else {
      return { success: false, message: 'Invalid code. Please try again.' };
    }
  }

  // Check if code exists and is valid
  hasValidCode(identifier: string): boolean {
    const stored = this.storedCodes.get(identifier);
    if (!stored) return false;
    
    const now = Date.now();
    const fiveMinutes = 5 * 60 * 1000;
    return (now - stored.timestamp <= fiveMinutes) && (stored.attempts < 3);
  }
}
