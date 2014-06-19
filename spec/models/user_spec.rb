require_relative '../spec_helper'

describe "User Model" do

  describe "User create" do

    before do
      @user = User.create!(
        :first_name => "Damola",
        :last_name => "Omotosho"
        )
    end

    it "should have the fields first_name and last_name" do
      expect(@user.first_name).to eq "Damola"
      expect(@user.last_name).to eq "Omotosho"
    end
  end
end

describe User do


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

  it "has a name" do
    expect(@user.name).to eq "Matt"
  end

  it "can have multiple medications" do
    med1 = Medication.create(:user_id => @user.id)
    med2 = Medication.create(:user_id => @user.id)
    expect(@user.medications).to eq [med1, med2]
  end

end
