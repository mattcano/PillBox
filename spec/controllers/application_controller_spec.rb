require_relative '../spec_helper'

describe Devise::SessionsController do

  describe "after_sign_in_path_for" do
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

    it "takes the user to /mypillbox after successful sign-in" do
      @request.env["devise.mapping"] = Devise.mappings[:user]
      post :create, {:user => {:email => 'mail@gmail.com', :password => 'password123'} }
      expect(response).to redirect_to '/mypillbox'
    end

  end

end