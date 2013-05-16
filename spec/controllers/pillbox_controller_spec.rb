require_relative '../spec_helper'

describe PillboxController do

  describe "update_meds_array" do
    include Devise::TestHelpers

    before do 
      @user = User.create!(
        :name => "Matt",
        :email => "mail@gmail.com",
        :password => "password123",
        :password_confirmation => "password123"
      )
    end

    after do
      User.destroy_all
    end

    it "creates an alphabetized array of a users medications, sorted by name" do
      sign_in @user
      med1 = Medication.create(:user_id => @user.id, :name => "Omega")
      med2 = Medication.create(:user_id => @user.id, :name => "Alpha")
      get :mypillbox
      expect(assigns(:meds_array)).to eq [med2, med1] 
      expect(assigns(:counter)).to eq 1
    end

  end

end
