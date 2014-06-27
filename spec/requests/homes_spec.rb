require_relative '../spec_helper'

describe "Home page" do

  describe "Jumbotron partial" do
    it "should have the content 'Keep your loved ones healthy'" do
      visit '/'
      expect(page).to have_content('Keep your loved ones healthy')
    end
  end

  describe "Landing partial" do
    it "should have the content 'Focus on Life'" do
      visit '/'
      expect(page).to have_content('Focus on Life')
    end

    it "should have the content 'Family-Social Medication Reminders'" do
      visit '/'
      expect(page).to have_content('Family-Social Medication Reminders')
    end
    
    it "should have the content 'mutual responsibility'" do
      visit '/'
      expect(page).to have_content('mutual responsibility')
    end
    

  end
end