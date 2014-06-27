module ApplicationHelper

  def resource_name
    :user
  end
 
  def resource
    @resource ||= User.new
  end
 
  def devise_mapping
    @devise_mapping ||= Devise.mappings[:user]
  end 

  def full_title(page_title)
  	base_title = "PillBox"
  	if page_title.empty?
  		base_title
  	else
  		"#{base_title} | #{page_title}"
  	end
  end
end
