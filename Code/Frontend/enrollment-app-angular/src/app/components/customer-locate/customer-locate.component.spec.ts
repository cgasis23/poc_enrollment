import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CustomerLocateComponent } from './customer-locate.component';
import { FormsModule } from '@angular/forms';
import { By } from '@angular/platform-browser';

describe('CustomerLocateComponent', () => {
  let component: CustomerLocateComponent;
  let fixture: ComponentFixture<CustomerLocateComponent>;

  const mockFormData = {
    accountNumber: '',
    ssn: '',
    birthdate: '',
    email: '',
    username: '',
    password: '',
    confirmPassword: '',
    phoneNumber: ''
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CustomerLocateComponent, FormsModule]
    }).compileComponents();

    fixture = TestBed.createComponent(CustomerLocateComponent);
    component = fixture.componentInstance;
    component.formData = mockFormData;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize with empty form state', () => {
    expect(component.formState().accountNumber).toBe('');
    expect(component.formState().ssn).toBe('');
    expect(component.formState().birthdate).toBe('');
    expect(component.formState()._ssnDigits).toBe('');
  });

  it('should initialize with empty errors', () => {
    expect(component.errors()).toEqual({});
  });

  describe('Input Handling', () => {
    describe('handleInputChange', () => {
      it('should format account number with spaces every 4 digits', () => {
        component.handleInputChange('accountNumber', '1234567890123456');
        expect(component.formState().accountNumber).toBe('1234 5678 9012 3456');
      });

      it('should limit account number to 16 digits', () => {
        component.handleInputChange('accountNumber', '12345678901234567890');
        expect(component.formState().accountNumber).toBe('1234 5678 9012 3456');
      });

      it('should remove non-digits from account number', () => {
        component.handleInputChange('accountNumber', '1234-5678-9012-3456');
        expect(component.formState().accountNumber).toBe('1234 5678 9012 3456');
      });

      it('should handle birthdate input', () => {
        const testDate = '1990-01-01';
        component.handleInputChange('birthdate', testDate);
        expect(component.formState().birthdate).toBe(testDate);
      });

      it('should clear error when user starts typing', () => {
        // Set an error first
        component.errors.set({ accountNumber: 'Account number is required' });
        
        component.handleInputChange('accountNumber', '1234');
        
        expect(component.errors().accountNumber).toBe('');
      });
    });

    describe('handleSSNInput', () => {
      it('should format SSN with dashes correctly', () => {
        component.handleSSNInput('123456789');
        expect(component.formState().ssn).toBe('123-45-6789');
        expect(component.formState()._ssnDigits).toBe('123456789');
      });

      it('should handle partial SSN input', () => {
        component.handleSSNInput('123');
        expect(component.formState().ssn).toBe('123');
        expect(component.formState()._ssnDigits).toBe('123');
      });

      it('should handle SSN with 5 digits', () => {
        component.handleSSNInput('12345');
        expect(component.formState().ssn).toBe('123-45');
        expect(component.formState()._ssnDigits).toBe('12345');
      });

      it('should limit SSN to 9 digits', () => {
        component.handleSSNInput('123456789012345');
        expect(component.formState().ssn).toBe('123-45-6789');
        expect(component.formState()._ssnDigits).toBe('123456789');
      });

      it('should remove non-digits from SSN', () => {
        component.handleSSNInput('123-45-6789');
        expect(component.formState().ssn).toBe('123-45-6789');
        expect(component.formState()._ssnDigits).toBe('123456789');
      });

      it('should clear SSN error when user starts typing', () => {
        component.errors.set({ ssn: 'SSN is required' });
        
        component.handleSSNInput('123');
        
        expect(component.errors().ssn).toBe('');
      });
    });
  });

  describe('Validation', () => {
    describe('validateSSN', () => {
      it('should validate correct SSN format', () => {
        expect(component.validateSSN('123456789')).toBe(true);
      });

      it('should reject SSN with less than 9 digits', () => {
        expect(component.validateSSN('12345678')).toBe(false);
      });

      it('should reject SSN with more than 9 digits', () => {
        expect(component.validateSSN('1234567890')).toBe(false);
      });

      it('should reject SSN with all same digits', () => {
        expect(component.validateSSN('111111111')).toBe(false);
      });

      it('should reject SSN starting with 000', () => {
        expect(component.validateSSN('000123456')).toBe(false);
      });

      it('should reject SSN starting with 666', () => {
        expect(component.validateSSN('666123456')).toBe(false);
      });

      it('should reject SSN starting with 900-999', () => {
        expect(component.validateSSN('900123456')).toBe(false);
        expect(component.validateSSN('999123456')).toBe(false);
      });

      it('should reject SSN with middle digits as 00', () => {
        expect(component.validateSSN('123004567')).toBe(false);
      });

      it('should reject 000000000', () => {
        expect(component.validateSSN('000000000')).toBe(false);
      });
    });

    describe('validateBirthdate', () => {
      it('should validate valid birthdate for 18+ year old', () => {
        const validDate = new Date();
        validDate.setFullYear(validDate.getFullYear() - 25);
        const dateString = validDate.toISOString().split('T')[0];
        
        expect(component.validateBirthdate(dateString)).toBe(true);
      });

      it('should reject empty birthdate', () => {
        expect(component.validateBirthdate('')).toBe(false);
      });

      it('should reject future date', () => {
        const futureDate = new Date();
        futureDate.setFullYear(futureDate.getFullYear() + 1);
        const dateString = futureDate.toISOString().split('T')[0];
        
        expect(component.validateBirthdate(dateString)).toBe(false);
      });

      it('should reject invalid date format', () => {
        expect(component.validateBirthdate('invalid-date')).toBe(false);
      });

      it('should reject under 18 years old', () => {
        const youngDate = new Date();
        youngDate.setFullYear(youngDate.getFullYear() - 17);
        const dateString = youngDate.toISOString().split('T')[0];
        
        expect(component.validateBirthdate(dateString)).toBe(false);
      });

      it('should accept exactly 18 years old', () => {
        const eighteenYearsAgo = new Date();
        eighteenYearsAgo.setFullYear(eighteenYearsAgo.getFullYear() - 18);
        const dateString = eighteenYearsAgo.toISOString().split('T')[0];
        
        expect(component.validateBirthdate(dateString)).toBe(true);
      });
    });

    describe('validateForm', () => {
      it('should return true for valid form data', () => {
        component.formState.set({
          accountNumber: '1234 5678 9012 3456',
          ssn: '123-45-6789',
          birthdate: '1990-01-01',
          _ssnDigits: '123456789'
        });

        const isValid = component.validateForm();
        expect(isValid).toBe(true);
        expect(component.errors()).toEqual({});
      });

      it('should return false and set errors for empty account number', () => {
        component.formState.set({
          accountNumber: '',
          ssn: '123-45-6789',
          birthdate: '1990-01-01',
          _ssnDigits: '123456789'
        });

        const isValid = component.validateForm();
        expect(isValid).toBe(false);
        expect(component.errors().accountNumber).toBe('Account number is required');
      });

      it('should return false for invalid account number length', () => {
        component.formState.set({
          accountNumber: '1234 5678 9012',
          ssn: '123-45-6789',
          birthdate: '1990-01-01',
          _ssnDigits: '123456789'
        });

        const isValid = component.validateForm();
        expect(isValid).toBe(false);
        expect(component.errors().accountNumber).toBe('Account number must be exactly 16 digits');
      });

      it('should return false for invalid SSN', () => {
        component.formState.set({
          accountNumber: '1234 5678 9012 3456',
          ssn: '123-45-6789',
          birthdate: '1990-01-01',
          _ssnDigits: '111111111'
        });

        const isValid = component.validateForm();
        expect(isValid).toBe(false);
        expect(component.errors().ssn).toBe('Please enter a valid SSN');
      });

      it('should return false for invalid birthdate', () => {
        component.formState.set({
          accountNumber: '1234 5678 9012 3456',
          ssn: '123-45-6789',
          birthdate: '2025-01-01',
          _ssnDigits: '123456789'
        });

        const isValid = component.validateForm();
        expect(isValid).toBe(false);
        expect(component.errors().birthdate).toBe('You must be at least 18 years old and birthdate cannot be in the future');
      });
    });
  });

  describe('Form Submission', () => {
    it('should emit form data on valid submission', () => {
      spyOn(component.onNext, 'emit');
      
      component.formState.set({
        accountNumber: '1234 5678 9012 3456',
        ssn: '123-45-6789',
        birthdate: '1990-01-01',
        _ssnDigits: '123456789'
      });

      const mockEvent = { preventDefault: jasmine.createSpy('preventDefault') };
      component.handleSubmit(mockEvent as any);

      expect(mockEvent.preventDefault).toHaveBeenCalled();
      expect(component.onNext.emit).toHaveBeenCalledWith({
        accountNumber: '1234 5678 9012 3456',
        ssn: '123456789',
        birthdate: '1990-01-01'
      });
    });

    it('should not emit on invalid form', () => {
      spyOn(component.onNext, 'emit');
      
      component.formState.set({
        accountNumber: '',
        ssn: '',
        birthdate: '',
        _ssnDigits: ''
      });

      const mockEvent = { preventDefault: jasmine.createSpy('preventDefault') };
      component.handleSubmit(mockEvent as any);

      expect(mockEvent.preventDefault).toHaveBeenCalled();
      expect(component.onNext.emit).not.toHaveBeenCalled();
    });
  });

  describe('Template Integration', () => {
    it('should display account number input with proper binding', () => {
      component.formState.set({
        accountNumber: '1234 5678 9012 3456',
        ssn: '',
        birthdate: '',
        _ssnDigits: ''
      });
      fixture.detectChanges();

      const accountInput = fixture.debugElement.query(By.css('#accountNumber'));
      expect(accountInput.nativeElement.value).toBe('1234 5678 9012 3456');
    });

    it('should display SSN input with proper binding', () => {
      component.formState.set({
        accountNumber: '',
        ssn: '123-45-6789',
        birthdate: '',
        _ssnDigits: '123456789'
      });
      fixture.detectChanges();

      const ssnInput = fixture.debugElement.query(By.css('#ssn'));
      expect(ssnInput.nativeElement.value).toBe('123-45-6789');
    });

    it('should display birthdate input with proper binding', () => {
      component.formState.set({
        accountNumber: '',
        ssn: '',
        birthdate: '1990-01-01',
        _ssnDigits: ''
      });
      fixture.detectChanges();

      const birthdateInput = fixture.debugElement.query(By.css('#birthdate'));
      expect(birthdateInput.nativeElement.value).toBe('1990-01-01');
    });

    it('should show error messages when validation fails', () => {
      component.errors.set({
        accountNumber: 'Account number is required',
        ssn: 'SSN is required',
        birthdate: 'Birthdate is required'
      });
      fixture.detectChanges();

      const errorMessages = fixture.debugElement.queryAll(By.css('.text-red-500'));
      expect(errorMessages.length).toBe(3);
    });

    it('should show success messages when validation passes', () => {
      component.formState.set({
        accountNumber: '1234 5678 9012 3456',
        ssn: '123-45-6789',
        birthdate: '1990-01-01',
        _ssnDigits: '123456789'
      });
      fixture.detectChanges();

      const successMessages = fixture.debugElement.queryAll(By.css('.text-green-600'));
      expect(successMessages.length).toBe(3);
    });

    it('should disable submit button when form is incomplete', () => {
      component.formState.set({
        accountNumber: '',
        ssn: '',
        birthdate: '',
        _ssnDigits: ''
      });
      fixture.detectChanges();

      const submitButton = fixture.debugElement.query(By.css('button[type="submit"]'));
      expect(submitButton.nativeElement.disabled).toBe(true);
    });

    it('should enable submit button when form is complete', () => {
      component.formState.set({
        accountNumber: '1234 5678 9012 3456',
        ssn: '123-45-6789',
        birthdate: '1990-01-01',
        _ssnDigits: '123456789'
      });
      fixture.detectChanges();

      const submitButton = fixture.debugElement.query(By.css('button[type="submit"]'));
      expect(submitButton.nativeElement.disabled).toBe(false);
    });
  });

  describe('Input Event Handling', () => {
    it('should handle account number input event', () => {
      spyOn(component, 'handleInputChange');
      
      const accountInput = fixture.debugElement.query(By.css('#accountNumber'));
      accountInput.nativeElement.value = '1234567890123456';
      accountInput.nativeElement.dispatchEvent(new Event('input'));

      expect(component.handleInputChange).toHaveBeenCalledWith('accountNumber', '1234567890123456');
    });

    it('should handle SSN input event', () => {
      spyOn(component, 'handleSSNInput');
      
      const ssnInput = fixture.debugElement.query(By.css('#ssn'));
      ssnInput.nativeElement.value = '123456789';
      ssnInput.nativeElement.dispatchEvent(new Event('input'));

      expect(component.handleSSNInput).toHaveBeenCalledWith('123456789');
    });

    it('should handle birthdate input event', () => {
      spyOn(component, 'handleInputChange');
      
      const birthdateInput = fixture.debugElement.query(By.css('#birthdate'));
      birthdateInput.nativeElement.value = '1990-01-01';
      birthdateInput.nativeElement.dispatchEvent(new Event('input'));

      expect(component.handleInputChange).toHaveBeenCalledWith('birthdate', '1990-01-01');
    });

    it('should handle form submission event', () => {
      spyOn(component, 'handleSubmit');
      
      const form = fixture.debugElement.query(By.css('form'));
      form.nativeElement.dispatchEvent(new Event('submit'));

      expect(component.handleSubmit).toHaveBeenCalled();
    });
  });
});
